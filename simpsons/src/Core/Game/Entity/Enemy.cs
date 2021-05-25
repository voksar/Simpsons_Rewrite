using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Newtonsoft.Json;

namespace simpsons.Core
{
    public abstract class Enemy : Entity
    {
        public enum Paths
        {
            
        }

        [JsonProperty]
        public int Health {get;set;}

        public Enemy(string TextureName, float X, float Y, int Health)
         : base(TextureName, X, Y, 0, 0)
        {
            this.Health = Health;
        }

        public abstract void Update(GameTime gameTime, GameWindow window, Player player);
        protected abstract void GeneratePath();
    }

    class Bart : Enemy
    {
        public new enum Paths 
        {
            Straight,
            Diagonal
        }


        public Bart(string TextureName, float X, float Y, int Health)
         : base(TextureName, X, Y, Health)
        {
            GeneratePath();
        }

        public override void Update(GameTime gameTime, GameWindow window, Player player)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            X += SpeedX * delta;
            Y += SpeedY * delta;


            if (X > window.ClientBounds.Width - Texture.Width || X < 0)
            {
                SpeedX *= -1;
            }
            
            if(Health <= 0)
                IsAlive = false;

            if (vector.Y > window.ClientBounds.Height + Texture.Height)
                IsAlive = false;
            
        }

        protected override void GeneratePath()
        {
            var rnd = new Random();
            Paths path = (Paths) rnd.Next(Enum.GetNames(typeof(Paths)).Length);

            float speedX, speedY;
            int X_min = 200;
            int X_max = 400;
            int Y_min = 200;
            int Y_max = 400;


            switch(path)
            {
                case Paths.Diagonal:
                        

                        

                        speedX = rnd.Next(X_min, X_max);
                        speedY = rnd.Next(Y_min, Y_max);

                        SpeedX = speedX;
                        SpeedY = speedY;

                    break;
                case Paths.Straight:
                    speedY = rnd.Next(Y_min, Y_max);

                    SpeedY = speedY;
                    break;
            }
        }
    }
    class Burns : Enemy
    {
        public Burns(string TextureName, float X, float Y, int Health)
         : base(TextureName, X, Y, Health)
        {
            
        }

        public override void Update(GameTime gameTime, GameWindow window, Player player)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            X += SpeedX * delta;
            Y += SpeedY * delta;


            if (X > window.ClientBounds.Width - Texture.Width || X < 0)
            {
                SpeedX *= -1;
            }
            

            if (vector.Y > window.ClientBounds.Height + Texture.Height)
                IsAlive = false;
        }
        protected override void GeneratePath()
        {
            throw new NotImplementedException();
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

        public Boss(string TextureName, float X, float Y, int Health, Type BossType) : 
        base(TextureName, X, Y, Health)
        {
            this.BossType = BossType;
        }
        public override void Update(GameTime gameTime, GameWindow window, Player player)
        {
            Console.WriteLine(this.BossType.ToString());
        }
        protected override void GeneratePath()
        {
            throw new NotImplementedException();
        }
    }
}