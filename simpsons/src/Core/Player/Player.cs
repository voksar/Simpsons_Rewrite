using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simpsons.src.Static;
using System;

namespace simpsons.src.Core
{
    class Player : Entity
    {
        public int Score {get;set;}
        public int Cash {get;set;}
        public Player(string referTexture, float x, float y) : base(referTexture,x,y,5,5)
        {
                        
        }
        public void Update()
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, vector, Color.White);
        }
    }
}