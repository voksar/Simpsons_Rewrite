using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using simpsons.Core.Handlers;
using simpsons.Core.Utils;
using simpsons.Core.Interfaces;
using System;
using System.Linq;
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
        float frame = 0;
        bool allowKeyboard = false;
        Color selectedColor;

        Texture2D baseIcon;
        Texture2D rectangleDisplayInfo;

        private string _deleteMessage;
        private float _durationOpacity = 1f;

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
            
            var measure = FontHandler.Fonts["Fonts\\Reno20"].MeasureString(gameHandler.GameID).X;
            //-10 from right border
            float x = 1040 - measure;
            
            currentY = 75 + (((int)(baseIcon.Height * 0.6) + 5) * displayGamesItems.Count);
            float y = currentY;
            DisplayGamesItem displayGamesItem = new DisplayGamesItem(gameHandler, x, y);
            displayGamesItems.Add(displayGamesItem);
        }
        public int Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            /*frame++;
            frame %= 30;*/
            frame += delta;
            frame %= 0.8f;
            if (frame <= 0.4)
                selectedColor = new Color(159, 255, 111);
            else
                selectedColor = Color.Yellow;
            Timer(gameTime);
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
                if(InputHandler.Press(Keys.D) && allowKeyboard)
                {


                    GameHandler _gameHandler = displayGamesItems[selected].Game;
                    Simpsons.RemoveGameHandler(_gameHandler);
                    for(int i = selected; i < displayGamesItems.Count; i++)
                    {
                        var tempRect = displayGamesItems[i].Rectangle;
                        Rectangle rect = new Rectangle(tempRect.X, tempRect.Y - 35 , tempRect.Width, tempRect.Height);
                        displayGamesItems[i].Rectangle = rect;
                    }

                    

                    displayGamesItems.RemoveAt(selected);
                    _durationOpacity = 1f;
                    _deleteMessage = $"Game {_gameHandler.GameID} deleted";

                    if(displayGamesItems.ElementAtOrDefault(selected) == null)
                    {
                        selected--;
                    }

                    if(displayGamesItems.Count == 0)
                        selected = 0;
                }
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
                    return StartStateChange(15, 3, 0, 400, gameTime);
                }
            }
            return (int)Simpsons.States.Saves;
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow window)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)RectangleX, 0, (int)RectangleWidth, window.ClientBounds.Height)
            , Color.White);
            var measure = FontHandler.Fonts["Fonts\\Reno20"].MeasureString("Saves").Length();
            float x = (window.ClientBounds.Width / 2) - (measure / 2);
            Utilities.DrawOutlineText("Fonts\\Reno20",spriteBatch, "Saves", new Vector2(x, 10), Color.White, Opacity);

            if(_deleteMessage != null)
            {
                var measureDelete = FontHandler.Fonts["Fonts\\Reno14"].MeasureString(_deleteMessage).Length();
                float xDelete = (window.ClientBounds.Width / 2) - (measureDelete / 2);
                Utilities.DrawOutlineText("Fonts\\Reno14",spriteBatch, _deleteMessage, new Vector2(xDelete, 45), Color.Red, _durationOpacity);
            }
            for(int i = 0; i < displayGamesItems.Count; i++)
            {
                spriteBatch.Draw(baseIcon, new Vector2(
                    560, displayGamesItems[i].Rectangle.Y
                ), null, Color.White * Opacity, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                if(i == selected)
                {
                    Utilities.DrawOutlineText(spriteBatch, displayGamesItems[i].DisplayID,new Vector2(displayGamesItems[i].Rectangle.X,displayGamesItems[i].Rectangle.Y),selectedColor, Opacity);
                }
                else 
                {
                    Utilities.DrawOutlineText(spriteBatch, displayGamesItems[i].DisplayID,
                    new Vector2(displayGamesItems[i].Rectangle.X,displayGamesItems[i].Rectangle.Y),
                    Color.White, Opacity);
                }
            }
        }
        
        private void Timer(GameTime gameTime)
        {
            if(_deleteMessage != null)
            {
                _durationOpacity -= (float)gameTime.ElapsedGameTime.TotalSeconds / 2;
                if(_durationOpacity <= 0)
                {
                    _deleteMessage = null;
                    _durationOpacity = 1f;

                }    
            }
            
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
            RectangleX = 550;
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