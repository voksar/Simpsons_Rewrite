using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using simpsons.Core.Handlers;
using simpsons.Core.Helpers;
using simpsons.Core.Interfaces;
using System;
namespace simpsons.Core
{
    class DisplayGames : IChangeState
    {
        public List<DisplayGamesItem> displayGamesItems{get;set;}
        
        //Interface properties
        public Texture2D Texture {get;set;}
        public bool IsOpacityDone {get;set;}
        public bool IsChangingState {get;set;}
        public int RectangleX {get;set;}
        public int RectangleWidth {get;set;}
        public float Opacity {get;set;}        

        int selected = 0;
        int currentY = 10;
        
        public DisplayGames()
        {
            displayGamesItems = new List<DisplayGamesItem>();
            IsOpacityDone = false;
            IsChangingState = false;
            RectangleX = 250;
            RectangleWidth = 500;
            Opacity = 0.0f;
        }
        public void LoadContent(GameWindow window, GraphicsDevice graphicsDevice)
        {
            Texture = Helper.RectangleCreator(RectangleWidth, window.ClientBounds.Height,
            graphicsDevice, Color.Black, 0.8f);
        }
        public void AddGameItem(GameHandler gameHandler)
        {
            float x = 60;
            float y = 10 + currentY;

            currentY += 30;

            DisplayGamesItem displayGamesItem = new DisplayGamesItem(gameHandler, x, y);
            displayGamesItems.Add(displayGamesItem);
        }
        public int Update()
        {
            
            
            if(!IsChangingState)
            {
                if(Opacity + 0.05f <= 1.0f)
                    Opacity += 0.05f;
                if(InputHandler.Press(Keys.Down))
                    if(selected < displayGamesItems.Count - 1)
                        selected++;
                if(InputHandler.Press(Keys.Up))
                    if(selected > 0)
                        selected--;
                if(InputHandler.Press(Keys.Enter))
                    return (int)Simpsons.StartGame(displayGamesItems[selected].Game);
                if(InputHandler.GoBackPressed())
                {   
                    IsChangingState = true;
                }
                    
            }
            if(IsChangingState)
            {
                
                if(!IsOpacityDone)
                    Opacity = Opacity - 0.1f;
                if(Opacity <= 0 && !IsOpacityDone)
                    IsOpacityDone = true;
                if(IsOpacityDone)
                {
                    return StartStateChange(10, 4, 0, 400);
                }
            }
            return (int)Simpsons.States.Saves;
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow window)
        {
            spriteBatch.Draw(Texture, new Rectangle(RectangleX, 0, RectangleWidth, window.ClientBounds.Height)
            , Color.White);
            for(int i = 0; i < displayGamesItems.Count; i++)
            {
                if(i == selected)
                {
                    Helper.DrawOutlineText(spriteBatch, displayGamesItems[i].DisplayID,
                    new Vector2(displayGamesItems[i].Rectangle.X,displayGamesItems[i].Rectangle.Y),
                    Color.Green, Opacity);
                }
                else 
                {
                    Helper.DrawOutlineText(spriteBatch, displayGamesItems[i].DisplayID,
                    new Vector2(displayGamesItems[i].Rectangle.X,displayGamesItems[i].Rectangle.Y),
                    Color.White, Opacity);
                }
            }
        }
        public int StartStateChange(int amountX, int amountWidth, int targetX, int targetWidth)
        {
            //Console.WriteLine($"Target X: {targetX} \n Current X: {rectangleX} \n Target Width: {targetWidth} \n Current Width: {rectangleWidth}");
            RectangleX -= amountX;
            RectangleWidth -= amountWidth;
            if(RectangleWidth == targetWidth && RectangleX == targetX)
            {
                
                Opacity = 0.0f;
                IsOpacityDone = false;
                IsChangingState = false;
                RectangleX = 250;
                RectangleWidth = 500;
                return (int)Simpsons.States.Menu;
            }
            return (int)Simpsons.States.Saves;
        }
    }
    class DisplayGamesItem
    {
        public string DisplayID {get;set;}
        public Rectangle Rectangle {get;set;}
        public GameHandler Game{get;set;}
        public DisplayGamesItem(GameHandler gameHandler,float x, float y)
        {
            Rectangle = new Rectangle((int)x,(int)y, 250, 100);
            DisplayID = gameHandler.GameID;
            Game = gameHandler;
        }
    }
}