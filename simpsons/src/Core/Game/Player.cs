using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using simpsons.Core.Handlers;
using Newtonsoft.Json;

namespace simpsons.Core
{
    public class Player : Entity
    {
        [JsonProperty]
        public string BulletName {get;set;}
        public Texture2D BulletTexture {get;set;}


        public Player(string TextureName, float X, float Y, float SpeedX, float SpeedY, string BulletName)
         : base(TextureName, X, Y, SpeedX, SpeedY)
        {
            this.BulletName = BulletName;
            BulletTexture = TextureHandler.Sprites[BulletName];
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
            
            
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, vector, Color.White);
        }
    }
}