using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simpsons.src.Static;
using System;

namespace simpsons.src.Core
{
    abstract class Enemy : Entity
    {
        public int Health {get;set;}
        public Enemy(string referTexture, float x, float y, float speedX, float speedY, int health):
        base(referTexture,x,y,speedX,speedY)
        {
            Health = health;
        }

        public abstract void Update(GameTime gameTime, GameWindow window, Player player);
    }

    class Bart : Enemy
    {
        public Bart(string referTexture, float x, float y, float speedX, float speedY, int health):
        base(referTexture,x,y,speedX,speedY,health)
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