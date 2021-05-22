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
        //Interface properties
        public Texture2D Texture {get;set;}
        public bool IsOpacityDone {get;set;}
        public bool IsChangingState {get;set;}
        public float RectangleX {get;set;}
        public float RectangleWidth {get;set;}
        public float Opacity {get;set;} 

        public Store()
        {
            LoadStateChangeVariables();
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
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
    }

    class StoreItem
    {

        public StoreItem(Texture2D texture, Vector2 position, int state, string name)
        {
            Texture = texture;
            Position = position;
            State = state;
            Name = name;
        }

        public Texture2D Texture {get;set;}
        public Vector2 Position {get;set;}
        public int State {get;set;}
        public string Name {get;set;}
    }
}