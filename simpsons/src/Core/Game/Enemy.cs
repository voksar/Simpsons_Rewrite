using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Newtonsoft.Json;

namespace simpsons.Core
{
    public abstract class Enemy : Entity
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
            Y += SpeedY;


            if (X > window.ClientBounds.Width - Texture.Width || X < 0)
            {
                SpeedX *= -1;
            }
            

            if (vector.Y > window.ClientBounds.Height + Texture.Height)
                IsAlive = false;
            //Remove object if not alive
            if(!IsAlive)
                Simpsons.Enemies.Remove(this);
        }
    }
    class Burns : Enemy
    {
        public Burns(string TextureName, float X, float Y, float SpeedX, float SpeedY, int Health)
         : base(TextureName, X, Y, SpeedX, SpeedY, Health)
        {
            
        }

        public override void Update(GameTime gameTime, GameWindow window, Player player)
        {
            X += SpeedX;
            Y += SpeedY;


            if (X > window.ClientBounds.Width - Texture.Width || X < 0)
            {
                SpeedX *= -1;
            }
            

            if (vector.Y > window.ClientBounds.Height + Texture.Height)
                IsAlive = false;
            //Remove object if not alive
            if(!IsAlive)
                Simpsons.Enemies.Remove(this);
        }
    }
    public class Boss : Enemy
    {
        public enum Type
        {
            Maggie,
            Wiggum
        }

        [JsonProperty]
        public Type BossType {get;set;}

        public Boss(string TextureName, float X, float Y, float SpeedX, float SpeedY, int Health, Type BossType) : 
        base(TextureName, X, Y, SpeedX, SpeedY, Health)
        {
            this.BossType = BossType;
        }
        public override void Update(GameTime gameTime, GameWindow window, Player player)
        {
            Console.WriteLine(this.BossType.ToString());
        }
    }
}