using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using simpsons.Core.Handlers;
using simpsons.Core.Utils;
using simpsons.Core.Interfaces;
using System;
namespace simpsons.Core
{
    public class DisplayGames : IChangeState
    {
        public List<DisplayGamesItem> displayGamesItems{get;set;}
        
        //Interface properties
        public Texture2D Texture {get;set;}
        public bool IsOpacityDone {get;set;}
        public bool IsChangingState {get;set;}
        public float RectangleX {get;set;}
        public float RectangleWidth {get;set;}
        public float Opacity {get;set;}        



        int selected = 0;
        int currentY = 0;
        int frame = 0;
        bool allowKeyboard = false;
        Color selectedColor;

        Texture2D baseIcon;
        Texture2D rectangleDisplayInfo;
        
        public DisplayGames()
        {
            displayGamesItems = new List<DisplayGamesItem>();
            LoadStateChangeVariables();
        }
        public void LoadContent(GameWindow window, GraphicsDevice graphicsDevice)
        {
            Texture = Utilities.RectangleCreator((int)RectangleWidth, window.ClientBounds.Height,
            graphicsDevice, Color.Black, 0.8f);
            baseIcon = TextureHandler.Sprites["MenuIcons\\Saves"];
            rectangleDisplayInfo = Utilities.RectangleCreator(400, 300, graphicsDevice, Color.Black, 0.9f
            );
        }
        public void AddGameItem(GameHandler gameHandler)
        {
            
            var measure = FontHandler.Fonts["Reno20"].MeasureString(gameHandler.GameID).X;
            float x = 740 - measure;
            
            currentY = 50 + (((int)(baseIcon.Height * 0.6) + 5) * displayGamesItems.Count);
            float y = currentY;
            DisplayGamesItem displayGamesItem = new DisplayGamesItem(gameHandler, x, y);
            displayGamesItems.Add(displayGamesItem);
        }
        public int Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frame++;
            frame %= 30;
            if (frame <= 15)
                selectedColor = new Color(159, 255, 111);
            else
                selectedColor = Color.Yellow;
            
            if(!IsChangingState)
            {
                allowKeyboard = MouseHandler.CheckIfSameSpot();
                if(Opacity + (0.05f * delta * 60) <= 1.0f)
                    Opacity += (0.05f * delta * 60);
                if(InputHandler.Press(Keys.Down) && allowKeyboard)
                    if(selected < displayGamesItems.Count - 1)
                        selected++;
                if(InputHandler.Press(Keys.Up) && allowKeyboard)
                    if(selected > 0)
                        selected--;
                //Checks if user presses enter
                //or if the user presses mouse1 and is hovering over the correct object
                if(displayGamesItems.Count != 0)
                    if(InputHandler.Press(Keys.Enter) || (MouseHandler.MouseState.LeftButton == ButtonState.Pressed
                    && displayGamesItems[selected].Rectangle.Contains(MouseHandler.MouseState.X, MouseHandler.MouseState.Y)))
                        return (int)Simpsons.StartGame(displayGamesItems[selected].Game);

                if(InputHandler.GoBackPressed())
                {   
                    IsChangingState = true;
                }
                
                for(int i = 0; i < displayGamesItems.Count; i++)
                {
                    if(displayGamesItems[i].Rectangle.
                    Contains(MouseHandler.MouseState.X, MouseHandler.MouseState.Y)
                    && !allowKeyboard)
                    {
                        selected = i;
                    }                
                }
                    
            }
            if(IsChangingState)
            {
                
                if(!IsOpacityDone)
                    Opacity -= (0.1f * delta * 60);
                if(Opacity <= 0 && !IsOpacityDone)
                    IsOpacityDone = true;
                if(IsOpacityDone)
                {
                    return StartStateChange(10, 4, 0, 400, gameTime);
                }
            }
            return (int)Simpsons.States.Saves;
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow window)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)RectangleX, 0, (int)RectangleWidth, window.ClientBounds.Height)
            , Color.White);
            var measure = FontHandler.Fonts["Reno20"].MeasureString("Saves").Length();
            float x = (window.ClientBounds.Width / 2) - (measure / 2);
            Utilities.DrawOutlineText("Reno20",spriteBatch, "Saves", 
            new Vector2(x, 10), Color.White, Opacity);
            for(int i = 0; i < displayGamesItems.Count; i++)
            {
                spriteBatch.Draw(baseIcon, new Vector2(
                    260, displayGamesItems[i].Rectangle.Y
                ), null, Color.White * Opacity, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                if(i == selected)
                {
                    Utilities.DrawOutlineText(spriteBatch, displayGamesItems[i].DisplayID,
                    new Vector2(displayGamesItems[i].Rectangle.X,displayGamesItems[i].Rectangle.Y),
                    selectedColor, Opacity);
                }
                else 
                {
                    Utilities.DrawOutlineText(spriteBatch, displayGamesItems[i].DisplayID,
                    new Vector2(displayGamesItems[i].Rectangle.X,displayGamesItems[i].Rectangle.Y),
                    Color.White, Opacity);
                }
            }
            //DrawSaveInformation(spriteBatch, window);
        }
        
        public void DrawSaveInformation(SpriteBatch spriteBatch, GameWindow window)
        {
            spriteBatch.Draw(rectangleDisplayInfo, 
            new Vector2(300, window.ClientBounds.Height - rectangleDisplayInfo.Height - 25), Color.White * Opacity);
            var measure = FontHandler.Fonts["Reno24"].MeasureString("SAVEINFORMATION").Length();
            float x = (window.ClientBounds.Width / 2) - (measure / 2);
            spriteBatch.DrawString(FontHandler.Fonts["Reno24"], "SAVEINFORMATION",
            new Vector2(x, window.ClientBounds.Height - rectangleDisplayInfo.Height - 25), Color.White * Opacity);

            spriteBatch.DrawString(FontHandler.Fonts["Reno14"],
            "Enemies: " + displayGamesItems[selected].Game.Enemies.Count, 
            new Vector2(310, window.ClientBounds.Height - rectangleDisplayInfo.Height + 10), Color.Green * Opacity);
            spriteBatch.DrawString(FontHandler.Fonts["Reno14"],
            "Time: " + (int)displayGamesItems[selected].Game.TimeInGame, 
            new Vector2(310, window.ClientBounds.Height - rectangleDisplayInfo.Height + 30), Color.Green * Opacity);
            spriteBatch.DrawString(FontHandler.Fonts["Reno14"],
            "Score: " + displayGamesItems[selected].Game.Score, 
            new Vector2(310, window.ClientBounds.Height - rectangleDisplayInfo.Height + 50), Color.Green * Opacity);
            spriteBatch.DrawString(FontHandler.Fonts["Reno14"],
            "Last Played: " + displayGamesItems[selected].Game.LastPlayed.ToString(), 
            new Vector2(310, window.ClientBounds.Height - rectangleDisplayInfo.Height + 70), Color.Green * Opacity);
        }
        public int StartStateChange(int amountX, int amountWidth, int targetX, int targetWidth, GameTime gameTime)
        {
            float temporaryamountX = 0f;
            float temporaryamountWidth = 0f;
            

            temporaryamountX = (float)(amountX * gameTime.ElapsedGameTime.TotalSeconds * 60);;
            temporaryamountWidth = (float)(amountWidth * gameTime.ElapsedGameTime.TotalSeconds * 60);

            if(RectangleX - temporaryamountX <= targetX && RectangleWidth - temporaryamountWidth <= targetWidth)
            {
                RectangleX = targetX;
                RectangleWidth = targetWidth;
            }
            else
            {
                RectangleX -= temporaryamountX;
                RectangleWidth -= temporaryamountWidth;
            }
            if(RectangleWidth == targetWidth && RectangleX == targetX)
            {
                
                LoadStateChangeVariables();
                return (int)Simpsons.States.Menu;
            }
            return (int)Simpsons.States.Saves;
        }
        public void LoadStateChangeVariables()
        {
            IsOpacityDone = false;
            IsChangingState = false;
            RectangleX = 250;
            RectangleWidth = 500;
            Opacity = 0.0f;
        }
    }
    public class DisplayGamesItem
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