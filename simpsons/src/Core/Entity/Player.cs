using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using simpsons.Core.Handlers;
using simpsons.Core.Utils;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace simpsons.Core.Entities
{
    public class Player : PlayerEntity
    {
        public List<Bullet> Bullets {get;set;}
        public Player(string TextureName, float X, float Y, float SpeedX, float SpeedY, string BulletName, int Health)
         : base(TextureName, X, Y, SpeedX, SpeedY, BulletName, Health)
        {
            Bullets = new List<Bullet>();
        }
        public void Update(GameWindow window, GameTime gameTime)
        {
            //PLayermovement, use deltatime for movement
            if(X > 0)
                if(InputHandler.IsPressing(Keys.Left) || InputHandler.IsPressing(Keys.A))
                    X -= SpeedX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(X < window.ClientBounds.Width - Texture.Width)
                if(InputHandler.IsPressing(Keys.Right) || InputHandler.IsPressing(Keys.D))
                    X += SpeedX * (float)gameTime.ElapsedGameTime.TotalSeconds;;
            if(Y > 0)
                if(InputHandler.IsPressing(Keys.Up) || InputHandler.IsPressing(Keys.W))
                    Y -= SpeedY * (float)gameTime.ElapsedGameTime.TotalSeconds;;
            if(Y < window.ClientBounds.Height - Texture.Height)
                if(InputHandler.IsPressing(Keys.Down) || InputHandler.IsPressing(Keys.S))
                    Y += SpeedY * (float)gameTime.ElapsedGameTime.TotalSeconds;;

            if(InputHandler.Press(Keys.Space))
                Bullets.Add(new Bullet(
                    BulletName, X, Y, 0, -8f
                ));

            if(Health <= 0)
                IsAlive = false;

            foreach(Bullet bullet in Bullets.ToList())
            {
                bullet.Update(gameTime);
                if(!bullet.IsAlive)
                    Bullets.Remove(bullet);
            }
                
                    
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(TextureHandler.Sprites["Icons/Border01"], new Vector2(5,5), Color.White);
            for (int i = 1; i <= HealthMax; i++)
            {
                float x = 45 + 32 * i;
                if (i <= Health)
                    spriteBatch.Draw(TextureHandler.Sprites["Player/heart"], new Vector2(x, 5), Color.White);
                else
                    spriteBatch.Draw(TextureHandler.Sprites["Player/heartdead"], new Vector2(x, 5), Color.White);
            }
            spriteBatch.Draw(Texture, vector, Color.White);

            foreach(Bullet bullet in Bullets)
                bullet.Draw(spriteBatch);
        }
    }
}