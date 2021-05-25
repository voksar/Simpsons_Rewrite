using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using simpsons.Core.Handlers;
using simpsons.Core.Utils;
using simpsons.Core.Interfaces;

namespace simpsons.Core.UI
{
    class Menu : IState
    {
        
        //Interface properties
        public Texture2D Texture {get;set;}
        public bool IsOpacityDone {get;set;}
        public bool IsChangingState {get;set;}
        public float RectangleX {get;set;}
        public float RectangleWidth {get;set;}
        public float Opacity {get;set;}  




        List<MenuItem> menu;
        float frame = 0;
        int selected = 0;
        int prevselected;
        float currentHeight = 0;
        
        int defaultMenuState;
        
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
            Texture = Utilities.RectangleCreator((int)RectangleWidth, window.ClientBounds.Height, gd, Color.Black, 0.8f);
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
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!IsChangingState)
            {
                if(Opacity + (0.05f * 60 * delta) <= 1f)
                {
                    Opacity += (0.05f * 60 * delta);
                }
                if(Opacity >= 1f)
                    Opacity = 1f;

                frame += (float)gameTime.ElapsedGameTime.TotalSeconds;
                frame %= 0.8f;
                allowKeyboard = MouseHandler.CheckIfSameSpot();
                prevselected = selected;

                if(InputHandler.GoBackPressed())
                    return (int)Simpsons.States.Quit;

                if (InputHandler.Press(Keys.Down) && allowKeyboard)
                {
                    menu[selected].SinValue = 0;
                    selected++;
                    if (selected > menu.Count - 1)
                        selected = 0;
                }
                if (InputHandler.Press(Keys.Up) && allowKeyboard)
                {
                    menu[selected].SinValue = 0;
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
                if (menu[selected].cX - (6 * gameTime.ElapsedGameTime.TotalSeconds * 60f) != menu[selected].Position.X
                    && menu[selected].cX > menu[selected].Position.X)
                    menu[selected].cX -= (float)(6 * gameTime.ElapsedGameTime.TotalSeconds * 60f);
                Opacity -= (0.05f * 60 * delta);

                if(Opacity <= 0 && !IsOpacityDone)
                    IsOpacityDone = true;

                if(IsOpacityDone)
                {
                    switch (state)
                    {
                        case (int)Simpsons.States.Saves:
                            return StartStateChange(15, 3, 550, 500, gameTime);
                        case (int)Simpsons.States.Store:
                            return StartStateChange(15, 3, 550, 500, gameTime);
                        default:
                            IsChangingState = false;
                            return menu[selected].State;
                    }
                }
            }
            return defaultMenuState;
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow window)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)RectangleX, 0, (int)RectangleWidth, window.ClientBounds.Height), Color.White);
            for (int i = 0; i < menu.Count; i++)
            {
                var c = Color.White;
                //Ha frame baserad flashing på selected texten
                if (frame <= 0.4)
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

            var widthOfText = FontHandler.Fonts["Fonts/Reno20"].MeasureString(Simpsons.playerInformationHandler.Cash + "$");
            Vector2 positionCashText = new Vector2(400 - widthOfText.X, 0);
            spriteBatch.DrawString(FontHandler.Fonts["Fonts/Reno20"], Simpsons.playerInformationHandler.Cash + "$"
            , positionCashText, Color.White * Opacity);
        }
        public int StartStateChange(int increaseX, int increaseWidth, int targetX, int targetWidth, GameTime gameTime)
        {
            foreach (MenuItem m in menu)
            {
                if (!moveNew)
                {
                    m.cX -= (float)(15 * gameTime.ElapsedGameTime.TotalSeconds * 60);
                    if (m.cX + m.ItemTexture.Width + m.Texture.Width < 0)
                    {
                        moveNew = true;
                    }
                }
            }
            if (moveNew)
            {
                var temporaryIncreaseX = 0f;
                var temporaryIncreaseWidth = 0f;
                temporaryIncreaseX = (float)(increaseX * gameTime.ElapsedGameTime.TotalSeconds * 60);
                temporaryIncreaseWidth = (float)(increaseWidth * gameTime.ElapsedGameTime.TotalSeconds * 60);
                if(RectangleX + temporaryIncreaseX >= targetX && temporaryIncreaseWidth + RectangleWidth >= targetWidth)
                {
                    RectangleX = targetX;
                    RectangleWidth = targetWidth;
                }
                else
                {
                    RectangleX += temporaryIncreaseX;
                    RectangleWidth += temporaryIncreaseWidth;
                }
                //Console.WriteLine($"{RectangleWidth}, {RectangleX}");
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