using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using simpsons.Core.Handlers;
using simpsons.Core.Utils;
using simpsons.Core.Interfaces;

namespace simpsons.Core
{
    public class Store : IState
    {
        //Constants
        const int TargetX = 750;

        //private variable declarations
        private List<int> _currentWidth = new List<int>();
        private PlayerInformationHandler _playerInformationHandler;


        //Interface properties
        public Texture2D Texture {get;set;}
        public bool IsOpacityDone {get;set;}
        public bool IsChangingState {get;set;}
        public float RectangleX {get;set;}
        public float RectangleWidth {get;set;}
        public float Opacity {get;set;} 

        
        public Dictionary<int, List<StoreItem>> MainStore {get;set;}

        //Keeps track of what state the store is in
        private enum StateTracker { Left, Right, None }
        private StateTracker stateTracker = StateTracker.None;

        private Color _color;

        private Color _colorYellow = Color.Yellow;
        private Color _colorGreen = new Color(159, 255, 111);
        private Color _colorArrows;

        //Variable declarations
        Texture2D StoreRectangle;
        Texture2D RasterizerRectangle;
        Vector2 RasterizerPosition = new Vector2(750, 200);
        int defaultState;
        int selectedy = 0;
        int[] selected;

        float frame = 0;
        


        public Store(PlayerInformationHandler playerInformationHandler, int state)
        {
            defaultState = state;
            _playerInformationHandler = playerInformationHandler;

            MainStore = new Dictionary<int, List<StoreItem>>()
            {
                { 0 ,new List<StoreItem>() },
            };

            for(int i = 1; i <= MainStore.Count; i++)
            {
                _currentWidth.Add(0);
            }

            selected = new int[MainStore.Count];
            Array.Fill(selected, 0);

            LoadStateChangeVariables();
        }
        public void Load(GraphicsDevice graphicsDevice)
        {
            foreach(KeyValuePair<string, int> item in CharacterList.Characters)
            {
                AddItem(0, item.Key, item.Value);
            }

            StoreRectangle = Utilities.RectangleCreator((int)RectangleWidth, ResolutionUtils.Height,
            graphicsDevice, Color.Black, 0.8f);
            RasterizerRectangle = new Texture2D(graphicsDevice, 100, 100);
        }
        public int Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdatePriceColor();
            switch(stateTracker)
            {
                case StateTracker.Left:
                    foreach(StoreItem item in MainStore[selectedy])
                    {
                        
                        item.X += 5 * delta * 60;
                    }
                    /*
                    if(MainStore[selectedy][selectedVal[selectedy]].X == targetX)
                    */
                    if(MainStore[selectedy][selected[selectedy]].X >= TargetX)
                    {
                        stateTracker = StateTracker.None;
                        MainStore[selectedy][selected[selectedy]].X = TargetX;
                    }
                    break;

                case StateTracker.Right:
                    foreach(StoreItem item in MainStore[selectedy])
                    {
                        item.X -= 5 * delta * 60;
                    }
                    if(MainStore[selectedy][selected[selectedy]].X <= TargetX)
                    {
                        stateTracker = StateTracker.None;
                        MainStore[selectedy][selected[selectedy]].X = TargetX;
                    }
                    break;
                default:
                    break;
            }
            
            StoreItem selectedItem = MainStore[selectedy][selected[selectedy]];
            if(!selectedItem.Unlocked && _playerInformationHandler.Cash < selectedItem.Cost)
                _color = Color.Red; 
            else if(_playerInformationHandler.SelectedPlayer == selectedItem.Name)
                _color = Color.Green;
            else
                _color = Color.Yellow;
            
            if(!IsChangingState)
            {
                frame += delta;
                frame %= 0.8f;
                if (frame <= 0.4)
                    _colorArrows = _colorGreen;
                else
                    _colorArrows = _colorYellow;

                if(Opacity + (0.05f * 60 * delta) <= 1f)
                {
                    Opacity += (0.05f * 60 * delta);
                }
                if(Opacity >= 1f)
                    Opacity = 1f;

                if(InputHandler.Press(Keys.Right))
                {
                    if(MainStore[selectedy].Count - 1 > selected[selectedy] && stateTracker == StateTracker.None)
                    {
                        selected[selectedy]++;
                        stateTracker = StateTracker.Right;
                    }
                
                }
                if(InputHandler.Press(Keys.Left))
                {
                    if(selected[selectedy] > 0 && stateTracker == StateTracker.None)
                    {
                        stateTracker = StateTracker.Left;
                        selected[selectedy]--;
                    }

                }

                if(InputHandler.Press(Keys.Enter))
                {
                    var item = MainStore[selectedy][selected[selectedy]];
                    if(!item.Unlocked)
                    {
                        if(_playerInformationHandler.Cash >= item.Cost)
                        {
                            _playerInformationHandler.UnlockedPlayers.Add(item.Name);
                            _playerInformationHandler.Cash -= item.Cost;
                            _playerInformationHandler.SelectedPlayer = item.Name;
                            item.Unlocked = true;
                            Simpsons.Reload();
                        }
                    }
                    else
                    {
                        _playerInformationHandler.SelectedPlayer = item.Name;
                        Simpsons.Reload();
                        IsChangingState = true;
                    }

                }

                if(InputHandler.GoBackPressed() && stateTracker == StateTracker.None)
                    IsChangingState = true;

            }
            else 
            {
                Opacity -= (0.05f * 60 * delta);

                if(Opacity <= 0 && !IsOpacityDone)
                    IsOpacityDone = true;

                if(IsOpacityDone)
                {
                    return StartStateChange(15, 3, 0, 400, gameTime);
                }
            }

            

            return defaultState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(StoreRectangle, 
            new Rectangle((int)RectangleX, 0, (int)RectangleWidth, ResolutionUtils.Height), Color.White);
            
            
            spriteBatch.Draw(TextureHandler.Sprites["StoreIcons\\BorderSquare"], new Vector2(748, 198), Opacity * _color * 0.5f);
            if(selected[selectedy] > 0)
                spriteBatch.Draw(TextureHandler.Sprites["StoreIcons\\LeftArrow"], new Vector2(660, 200), _colorArrows * Opacity);
            if(selected[selectedy] < MainStore[selectedy].Count - 1)
                spriteBatch.Draw(TextureHandler.Sprites["StoreIcons\\RightArrow"], new Vector2(840, 200), _colorArrows * Opacity);

            spriteBatch.End();

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.ScissorTestEnable = true;
            spriteBatch.GraphicsDevice.RasterizerState = rasterizerState;
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)RasterizerPosition.X, (int)RasterizerPosition.Y, RasterizerRectangle.Width, RasterizerRectangle.Height);
            spriteBatch.Begin(rasterizerState: rasterizerState);

            foreach(KeyValuePair<int, List<StoreItem>> subStore in MainStore)
            {
                foreach(StoreItem item in subStore.Value)
                {
                    if(!item.Unlocked)
                    {
                        SpriteFont costFont = FontHandler.Fonts["Fonts\\Reno14"];
                        var costLength = costFont.MeasureString(item.Cost + "$");
                        float costX = (item.X + item.Texture.Width / 2) - (costLength.X / 2);
                        float costY = item.Y + item.Texture.Height - costLength.Y;

                        spriteBatch.Draw(item.Texture, new Vector2(item.X, item.Y), Color.Black * Opacity);

                        spriteBatch.DrawString(costFont, $"{item.Cost}$", new Vector2(costX, costY), item.CostColor * Opacity);
                        
                        if(_playerInformationHandler.Cash < item.Cost)
                            spriteBatch.Draw(TextureHandler.Sprites["StoreIcons\\Lock"], new Vector2(item.X, item.Y), item.Color * Opacity);
                    }
                    else
                    {
                        spriteBatch.Draw(item.Texture, new Vector2(item.X, item.Y), Color.White * Opacity);
                    }

                    
                }
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        }


        public int StartStateChange(int changeX, int changeWidth, int targetX, int targetWidth, GameTime gameTime)
        {
            float temporaryamountX = 0f;
            float temporaryamountWidth = 0f;
            

            temporaryamountX = (float)(changeX * gameTime.ElapsedGameTime.TotalSeconds * 60);
            temporaryamountWidth = (float)(changeWidth * gameTime.ElapsedGameTime.TotalSeconds * 60);

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
            return (int)Simpsons.States.Store;
        }

        public void LoadStateChangeVariables()
        {
            IsOpacityDone = false;
            IsChangingState = false;
            RectangleX = 550;
            RectangleWidth = 500;
            Opacity = 0.0f;
        }

        public void AddItem(int index, string name, int cost)
        {
            float x = 750 + _currentWidth[index];
            float y = 200 + (150 * index);
            bool unlocked = false;

            Vector2 position = new Vector2(x,y);

            string textureName = name.Replace("Player", "StoreIcons");
            _currentWidth[index] += TextureHandler.Sprites[textureName].Width;

            if(_playerInformationHandler.UnlockedPlayers.Contains(name))
                unlocked = true;

            MainStore[index].Add(new StoreItem(
                TextureHandler.Sprites[textureName], position, name, CharacterList.Characters[name], unlocked 
            ));
        }

        private void UpdatePriceColor()
        {
            foreach(StoreItem item in MainStore[selectedy])
            {
                if(item.Cost > _playerInformationHandler.Cash)
                    item.CostColor = Color.Red;
                else
                    item.CostColor = Color.Green;
            }
        }
    }

    public class StoreItem
    {

        public StoreItem(Texture2D texture, Vector2 position, string name, int cost, bool unlocked)
        {
            Texture = texture;
            Position = position;
            Name = name;
            X = position.X;
            Y = position.Y;
            Unlocked = unlocked;
            Cost = cost;

            if(!Unlocked)
                Color = new Color(54,69,79);
        }

        public Texture2D Texture {get;set;}
        public Vector2 Position {get;set;}
        public string Name {get;set;}
        public int Cost {get;set;}
        public bool Unlocked {get;set;}
        public Color Color {get;set;}
        public Color CostColor {get;set;}

        public float X {get;set;}
        public float Y {get;set;}
    }
}