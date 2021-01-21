using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using simpsons.Core.Handlers;
using simpsons.Core.Helpers;
using simpsons.Core.Interfaces;

namespace simpsons.Core
{
    class Menu
    {
        
        //Interface properties
        /*public Texture2D Texture {get;set;}
        public bool IsOpacityDone {get;set;}
        public bool IsChangingState {get;set;}
        public int RectangleX {get;set;}
        public int RectangleWidth {get;set;}
        public float Opacity {get;set;}  */ 




        SoundEffect soundEffect;
        List<MenuItem> menu;
        public int frame = 0;
        int selected = 0;
        int prevselected;
        float currentHeight = 0;
        MouseState mState;
        double lastChange = 0;
        Point p = new Point();
        int defaultMenuState;
        
        float temp = 0.0f;
        bool moveNew = false;
        MouseState prevmState = Mouse.GetState();
        bool allowKeyboard = false;
        KeyboardState prevState = Keyboard.GetState();
        bool changeState = false;
        int rectX = 0;
        int rectWidth = 400;
        bool loadInDone = true;

        float opacity = 1.0f;

        private GraphicsDevice gd;

        int state;
        Texture2D re;
        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
            prevselected = selected;
        }
        public void LoadContent(GraphicsDevice gd, GameWindow window, ContentManager content)
        {
            re = Helper.RectangleCreator(rectWidth, window.ClientBounds.Height, gd, Color.Black, 0.8f);
            soundEffect = content.Load<SoundEffect>("Menu/changeSelectSound");
            this.gd = gd;
            
        }
        public void AddItem(Texture2D itemTexture, int state, GameWindow window, Texture2D item)
        {
            float X = 60;
            float Y = 10 + currentHeight;

            currentHeight += itemTexture.Height + 5;
            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), state, item);
            menu.Add(temp);
        }
        public int Update(GameTime gameTime, MouseState mStatee, GameWindow window)
        {
            
            if (!changeState && loadInDone)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                prevmState = mState;

                for (int i = 0; i < menu.Count; i++)
                {
                    if (i == selected)
                    {

                        if (menu[i].cX + 2 != menu[i].Position.X + 30)
                        {

                            menu[i].cX += 2;
                            //Är endast ute efter de positiva värderna från Sin.
                            //Detta är mellan 0-1-0 men kör 0-1 ~ och sätter sedan värdet till det förbestämda.
                            menu[i].cY -= (float)Math.Sin(temp) / 2;
                            temp += 0.15f;
                        }
                        else
                        {
                            menu[i].cY = menu[i].Position.Y;
                            temp = 0.0f;
                        }
                    }
                    else
                    {
                        if (menu[i].cX - 2 != menu[i].Position.X && menu[i].cX > menu[i].Position.X)
                            menu[i].cX -= 2;
                        menu[i].cY = menu[i].Position.Y;
                    }
                }

                prevselected = selected;
                mState = mStatee;
                frame++;
                frame %= 30;
                if (prevmState.X == mState.X && prevmState.Y == mState.Y)
                {
                    allowKeyboard = true;
                }
                else
                    allowKeyboard = false;

                p.X = mState.X;
                p.Y = mState.Y;

                if (InputHandler.Press(Keys.Down))
                {
                    selected++;
                    if (selected > menu.Count - 1)
                        selected = 0;
                }
                if (InputHandler.Press(Keys.Up))
                {
                    selected--;
                    if (selected < 0)
                        selected = menu.Count - 1;
                    else if (selected > menu.Count - 1)
                        selected = 0;
                }
                for (int i = 0; i < menu.Count; ++i)
                {
                    if (menu[i].Rec.Contains(p) && !allowKeyboard)
                    {
                        selected = i;
                    }
                }

                prevState = keyboardState;
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
                if (InputHandler.Press(Keys.Enter) || mState.LeftButton == ButtonState.Pressed && menu[selected].Rec.Contains(p))
                {
                    soundEffect.Play(0.01f, 0, 0);  
                    changeState = true;
                    state = menu[selected].State;

                }
            }
            else
            {
                if (menu[selected].cX - 6 != menu[selected].Position.X
                    && menu[selected].cX > menu[selected].Position.X)
                    menu[selected].cX -= 6;
                switch (state)
                {
                    case (int)Simpsons.States.Saves:
                        return StartStateChange(10, 4, 250, 500);
                    default:
                        changeState = false;
                        return menu[selected].State;
                }

            }
            if (prevselected != selected)
                soundEffect.Play(0.01f, 0, 0);
            return defaultMenuState;
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow window)
        {
            if (opacity + 0.1f <= 1.0f)
                opacity += 0.1f;
            spriteBatch.Draw(re, new Rectangle(rectX, 0, rectWidth, window.ClientBounds.Height), Color.White);
            for (int i = 0; i < menu.Count; i++)
            {
                var c = Color.White;
                //Ha frame baserad flashing på selected texten
                if (frame <= 15)
                    c = new Color(159, 255, 111);
                else
                    c = Color.Yellow;
                if (i == selected || menu[i].Rec.Contains(p) && !allowKeyboard)
                {
                    spriteBatch.Draw(menu[i].ItemTexture, new Vector2(menu[i].cX - 50, menu[i].cY), Color.White * opacity);
                    spriteBatch.Draw(menu[i].Texture, new Vector2(menu[i].cX, menu[i].cY), c * opacity);
                }
                else
                {
                    spriteBatch.Draw(menu[i].ItemTexture, new Vector2(menu[i].cX - 50, menu[i].cY), Color.White * opacity);
                    spriteBatch.Draw(menu[i].Texture, new Vector2(menu[i].cX, menu[i].cY), Color.White * opacity);
                }
            }
        }
        public int StartStateChange(int increaseX, int increaseWidth, int targetX, int targetWidth)
        {
            foreach (MenuItem m in menu)
            {
                if (!moveNew)
                {
                    m.cX -= 15;
                    if (m.cX + m.ItemTexture.Width + m.Texture.Width < 0)
                    {
                        moveNew = true;
                    }
                }
            }
            if (moveNew)
            {
                rectX += increaseX;
                rectWidth += increaseWidth;
                if (rectX == targetX && rectWidth == targetWidth)
                {
                    rectWidth = 400;
                    rectX = 0;
                    moveNew = false;
                    opacity = 0.0f;
                    foreach (MenuItem m in menu)
                        m.cX = m.Position.X;
                    changeState = false;
                    return state;
                    
                }
            }
            return defaultMenuState;
        }
    }
    class MenuItem
    {
        public MenuItem(Texture2D texture, Vector2 position, int currentState, Texture2D itemTexture)
        {
            Texture = texture;
            Position = position;
            State = currentState;
            cX = position.X;
            cY = position.Y;
            ItemTexture = itemTexture;
            Rec = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
        public Texture2D Texture {get;set;}
        public Texture2D ItemTexture {get;set;}
        public Vector2 Position {get;set;}
        public int State {get;set;}
        public Rectangle Rec{get;set;}
        public float cX {get;set;}
        public float cY {get;set;}
    }
}