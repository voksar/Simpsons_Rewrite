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
        int selected = 0;
        int currentY = 10;
        List<DisplayGamesItem> displayGamesItems;
        public DisplayGames()
        {
            displayGamesItems = new List<DisplayGamesItem>();
        }
        public void AddGameItem(string text)
        {
            float x = 60;
            float y = 10 + currentY;

            currentY += 25;

            DisplayGamesItem displayGamesItem = new DisplayGamesItem(text, x, y);
            displayGamesItems.Add(displayGamesItem);
        }
        public void Update()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < displayGamesItems.Count; i++)
            {
                Console.WriteLine($"{i == selected}");
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
        public DisplayGamesItem(string text,float x, float y)
        {
            Rectangle = new Rectangle((int)x,(int)y, 250, 100);
            DisplayID = text;
        }
    }
}