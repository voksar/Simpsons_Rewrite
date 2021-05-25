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
    
    public class Companion : PlayerEntity
    {
        //Link up the companion to the player
        [JsonProperty]
        public Player Player {get;set;}

        public List<Bullet> Bullets {get;set;}

        private Enemy _target;
        private double shotTimer = 0;
        
        private Vector2 directionToPlayer;

        public Companion(string TextureName, float X, float Y, float SpeedX, float SpeedY, string BulletName, int Health, Player Player)
        : base(TextureName, X, Y, SpeedX, SpeedY, BulletName, Health)
        {
            this.Player = Player;
            Bullets = new List<Bullet>();
        }
        #nullable enable
        public void Update(GameTime gameTime, ObservableCollection<Enemy>? enemies)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Variables for targets
            double targetX, targetY;

            //Locate enemies within certain distance
            float distance = ResolutionUtils.Height;
            
            //check if enemies can be found
            if(enemies != null)
                if(enemies.Count <= 0)
                    _target = null;

            //Locate closest enemy
            /*foreach(Enemy e in enemies.ToList())
            {
                if(Vector2.Distance(new Vector2(e.X, e.Y), vector) < distance)
                {
                    distance = Vector2.Distance(new Vector2(e.X, e.Y), vector);
                    _target = e;
                }
            }*/

            //Calculate angle based on speed and spawn a bullet
            if(_target != null)
            {
                Vector2 direction;
                targetX = _target.X + (_target.Texture.Width / 2);
                targetY = _target.Y + (_target.Texture.Width / 2);

                direction.X = ((float)targetX - X);
                direction.Y = ((float)targetY - Y);
                direction.Normalize();

                if(gameTime.TotalGameTime.TotalSeconds > shotTimer + 1)
                {
                    Bullets.Add(
                        new Bullet(BulletName, X, Y, direction.X * 27, direction.Y * 27
                        )
                    );
                    shotTimer = gameTime.TotalGameTime.TotalSeconds;
                }
            }

            directionToPlayer.X = X - Player.X;
            directionToPlayer.Y = Y - Player.Y;

            directionToPlayer.Normalize();
            
            if(!Utilities.BetweenRanges((int)Player.X - 80, (int)Player.X + 80, (int)X))
            {
                X -= directionToPlayer.X * SpeedX * delta;
            }
            if(!Utilities.BetweenRanges((int)Player.Y - 80, (int)Player.Y + 80, (int)Y))
            {
                Y -= directionToPlayer.Y * SpeedY * delta;
            }
            

            foreach(Bullet bullet in Bullets.ToList())
            {
                bullet.Update(gameTime);
                if(!bullet.IsAlive)
                    Bullets.Remove(bullet);
            }
        }
        #nullable disable
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D borderTexture = TextureHandler.Sprites["Icons/Border01"];
            var width = ResolutionUtils.Width;
            var height = ResolutionUtils.Height;

            var posX = width - 5 - borderTexture.Width;

            
            spriteBatch.Draw(borderTexture, new Vector2(posX,5), Color.White);
            for (int i = 1; i <= HealthMax; i++)
            {
                float x = (posX - 5) - 32 * i;
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