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

namespace simpsons.Core
{
    public class Store : IState
    {

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

        

        //Variable declarations
        Texture2D StoreRectangle;
        Texture2D RasterizerRectangle;
        Vector2 RasterizerPosition = new Vector2(450, 200);
        int defaultState;


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
            return defaultState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(StoreRectangle, 
            new Rectangle((int)RectangleX, 0, (int)RectangleWidth, ResolutionUtils.Height), Color.White);
            spriteBatch.Draw(TextureHandler.Sprites["StoreIcons\\BorderSquare"], new Vector2(448, 198), Color.White);
            spriteBatch.End();

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.ScissorTestEnable = true;
            spriteBatch.GraphicsDevice.RasterizerState = rasterizerState;
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)RasterizerPosition.X, (int)RasterizerPosition.Y, RasterizerRectangle.Width, RasterizerRectangle.Height);
            spriteBatch.Begin(rasterizerState: rasterizerState);

            foreach(StoreItem si in MainStore[0])
            {
                spriteBatch.Draw(si.Texture, si.Position, si.Color);
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
            float x = 450 + _currentWidth[index];
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
            XSwap = position.X;
            YSwap = position.Y;
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

        public float XSwap {get;set;}
        public float YSwap {get;set;}
    }
}