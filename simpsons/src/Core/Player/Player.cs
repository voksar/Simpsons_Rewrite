using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simpsons.src.Models;
using System;

namespace simpsons.src.Core
{
    class Player : Entity
    {
        public Player(Texture2D texture, float x, float y) : base(texture,x,y,5,5)
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