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
    class Menu : IChangeState
    {
        
        //Interface properties
        public Texture2D Texture {get;set;}
        public bool IsOpacityDone {get;set;}
        public bool IsChangingState {get;set;}
        public int RectangleX {get;set;}
        public int RectangleWidth {get;set;}
        public float Opacity {get;set;}  




        SoundEffect soundEffect;
        List<MenuItem> menu;
        public int frame = 0;
        int selected = 0;
        int prevselected;
        float currentHeight = 0;
        
        int defaultMenuState;
        
        float temp = 0.0f;
        bool moveNew = false;
        bool allowKeyboard = false;

        private GraphicsDevice gd;

        int state;
        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
            prevselected = selected;
            LoadStateChangeVariables();
        }
        public void LoadContent(GraphicsDevice gd, GameWindow window, ContentManager content)
        {
            Texture = Helper.RectangleCreator(RectangleWidth, window.ClientBounds.Height, gd, Color.Black, 0.8f);
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
        public int Update(GameTime gameTime,GameWindow window)
        {
            
            if (!IsChangingState)
            {
                if (Opacity + 0.1f <= 1.0f)
                    Opacity += 0.1f;
                frame++;
                frame %= 30;
                allowKeyboard = MouseHandler.CheckIfSameSpot();
                prevselected = selected;

                if (InputHandler.Press(Keys.Down) && allowKeyboard)
                {
                    selected++;
                    if (selected > menu.Count - 1)
                        selected = 0;
                }
                if (InputHandler.Press(Keys.Up) && allowKeyboard)
                {
                    selected--;
                    if (selected < 0)
                        selected = menu.Count - 1;
                    else if (selected > menu.Count - 1)
                        selected = 0;
                }
                for (int i = 0; i < menu.Count; ++i)
                {
                    if (menu[i].Rec.Contains(MouseHandler.MouseState.X, MouseHandler.MouseState.Y) 
                    && !allowKeyboard)
                    {
                        selected = i;
                    }
                }
                
                if (InputHandler.Press(Keys.Enter) || 
                MouseHandler.MouseState.LeftButton == ButtonState.Pressed
                && menu[selected].Rec.Contains(MouseHandler.MouseState.X, MouseHandler.MouseState.Y))
                {
                    soundEffect.Play(0.01f, 0, 0);  
                    IsChangingState = true;
                    state = menu[selected].State;

                }
                for (int i = 0; i < menu.Count; i++)
                {
                    if (i == selected)
                    {
                        if (menu[i].cX + (2 * 60f * gameTime.ElapsedGameTime.TotalSeconds) < menu[i].Position.X + 30) //15 frames to reach
                        {
                            menu[i].cX += (float)(2 * 60f * gameTime.ElapsedGameTime.TotalSeconds);
                            
                            menu[i].cY += (float)Math.Sin(menu[i].SinValue) * (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
                            
                            Console.WriteLine((float)Math.Sin(menu[i].SinValue));
                            menu[i].SinValue -= 0.15f * (float)(60 * gameTime.ElapsedGameTime.TotalSeconds);
                            
                        }
                        else
                        {
                            menu[i].cY = menu[i].Position.Y;
                            menu[i].SinValue = 0.0f;
                        }
                    }
                    else
                    {
                        if (menu[i].cX - (2 * 60f * gameTime.ElapsedGameTime.TotalSeconds) >= menu[i].Position.X)
                            menu[i].cX -= (float)(2 * 60f * gameTime.ElapsedGameTime.TotalSeconds);
                        menu[i].cY = menu[i].Position.Y;
                    }
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
                        IsChangingState = false;
                        return menu[selected].State;
                }

            }
            if (prevselected != selected)
                soundEffect.Play(0.01f, 0, 0);
            return defaultMenuState;
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow window)
        {
            spriteBatch.Draw(Texture, new Rectangle(RectangleX, 0, RectangleWidth, window.ClientBounds.Height), Color.White);
            for (int i = 0; i < menu.Count; i++)
            {
                var c = Color.White;
                //Ha frame baserad flashing på selected texten
                if (frame <= 15)
                    c = new Color(159, 255, 111);
                else
                    c = Color.Yellow;
                if (i == selected)
                {
                    spriteBatch.Draw(menu[i].ItemTexture, new Vector2(menu[i].cX - 50, menu[i].cY), Color.White * Opacity);
                    spriteBatch.Draw(menu[i].Texture, new Vector2(menu[i].cX, menu[i].cY), c * Opacity);
                }
                else
                {
                    spriteBatch.Draw(menu[i].ItemTexture, new Vector2(menu[i].cX - 50, menu[i].cY), Color.White * Opacity);
                    spriteBatch.Draw(menu[i].Texture, new Vector2(menu[i].cX, menu[i].cY), Color.White * Opacity);
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
                RectangleX += increaseX;
                RectangleWidth += increaseWidth;
                if (RectangleX == targetX && RectangleWidth == targetWidth)
                {
                    LoadStateChangeVariables();
                    moveNew = false;
                    foreach (MenuItem m in menu)
                        m.cX = m.Position.X;
                    
                    return state;
                    
                }
            }
            return defaultMenuState;
        }
        public void LoadStateChangeVariables()
        {
            IsOpacityDone = false;
            IsChangingState = false;
            RectangleX = 0;
            RectangleWidth = 400;
            Opacity = 0.0f;
        }
    }
    class MenuItem
    {
        public Texture2D Texture {get;set;}
        public Texture2D ItemTexture {get;set;}
        public Vector2 Position {get;set;}
        public int State {get;set;}
        public Rectangle Rec{get;set;}
        public float cX {get;set;}
        public float cY {get;set;}
        public float SinValue {get;set;}
        public MenuItem(Texture2D texture, Vector2 position, int currentState, Texture2D itemTexture)
        {
            Texture = texture;
            Position = position;
            State = currentState;
            cX = position.X;
            cY = position.Y;
            ItemTexture = itemTexture;
            Rec = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            SinValue = 0;
        }
    }
}