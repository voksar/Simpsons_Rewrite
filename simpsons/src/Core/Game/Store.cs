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

        
        private enum StateTracker { Left, Right, None }

        private StateTracker stateTracker = StateTracker.None;


        //Variable declarations
        Texture2D StoreRectangle;
        Texture2D RasterizerRectangle;
        Vector2 RasterizerPosition = new Vector2(750, 200);
        int defaultState;
        int selectedy = 0;
        int[] selected;


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
        public int Update()
        {
            switch(stateTracker)
            {
                case StateTracker.Left:
                    foreach(StoreItem item in MainStore[selectedy])
                    {
                        
                        item.X += 1;
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
                        item.X -= 1;
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

            return defaultState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(StoreRectangle, 
            new Rectangle((int)RectangleX, 0, (int)RectangleWidth, ResolutionUtils.Height), Color.White);
            spriteBatch.Draw(TextureHandler.Sprites["StoreIcons\\BorderSquare"], new Vector2(748, 198), Color.White * 0.5f);
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
                    spriteBatch.Draw(item.Texture, new Vector2(item.X, item.Y), item.Color);
                }
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        }


        public int StartStateChange(int changeX, int changeWidth, int targetX, int targetWidth, GameTime gameTime)
        {
            return 0;
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


            if(!Unlocked)
                Color = new Color(54,69,79);
            else
                Color = new Color(255,255,255);
        }

        public Texture2D Texture {get;set;}
        public Vector2 Position {get;set;}
        public string Name {get;set;}
        public int Cost {get;set;}
        public bool Unlocked {get;set;}
        public Color Color {get;set;}

        public float X {get;set;}
        public float Y {get;set;}
    }
}