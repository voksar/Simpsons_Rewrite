using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Newtonsoft.Json;

namespace simpsons
{
    abstract class Enemy : Entity
    {
        [JsonProperty]
        public int Health {get;set;}
        public Enemy(string TextureName, float X, float Y, float SpeedX, float SpeedY, int Health)
         : base(TextureName, X, Y, SpeedX, SpeedY)
        {
            this.Health = Health;
        }

        public abstract void Update(GameTime gameTime, GameWindow window, Player player);
    }

    class Bart : Enemy
    {
        public Bart(string TextureName, float X, float Y, float SpeedX, float SpeedY, int Health)
         : base(TextureName, X, Y, SpeedX, SpeedY, Health)
        {
            
        }

        public override void Update(GameTime gameTime, GameWindow window, Player player)
        {
            X += SpeedX;

            if (X > window.ClientBounds.Width - Texture.Width || X < 0)
            {
                SpeedX *= -1;
            }
            Y += SpeedY;
            if (vector.Y > window.ClientBounds.Height + Texture.Height)
            { IsAlive = false; }
        }
    }
}