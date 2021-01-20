using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using simpsons.Core.Handlers;
using System;
namespace simpsons.Core
{
    class DisplayGames
    {
        public List<DisplayGamesItem> displayGamesItems{get;set;}

        int selected = 0;
        int currentY = 10;
        
        public DisplayGames()
        {
            displayGamesItems = new List<DisplayGamesItem>();
        }
        public void AddGameItem(GameHandler gameHandler)
        {
            float x = 60;
            float y = 10 + currentY;

            currentY += 25;

            DisplayGamesItem displayGamesItem = new DisplayGamesItem(gameHandler, x, y);
            displayGamesItems.Add(displayGamesItem);
        }
        public int Update()
        {
            if(InputHandler.Press(Keys.Down))
                if(selected < displayGamesItems.Count - 1)
                    selected++;
            if(InputHandler.Press(Keys.Up))
                if(selected > 0)
                    selected--;
            if(InputHandler.Press(Keys.Enter))
                return (int)Simpsons.StartGame(displayGamesItems[selected].Game);

            
            return (int)Simpsons.States.Saves;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < displayGamesItems.Count; i++)
            {
                if(i == selected)
                {
                    spriteBatch.DrawString(FontHandler.Fonts["Reno20"], 
                    displayGamesItems[i].DisplayID, 
                    new Vector2(displayGamesItems[i].Rectangle.X, displayGamesItems[i].Rectangle.Y),
                     Color.Green);
                }
                else 
                {
                    spriteBatch.DrawString(FontHandler.Fonts["Reno20"], 
                    displayGamesItems[i].DisplayID, 
                    new Vector2(displayGamesItems[i].Rectangle.X, displayGamesItems[i].Rectangle.Y),
                     Color.Red);
                }
            }
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