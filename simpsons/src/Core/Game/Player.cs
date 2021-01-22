using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using simpsons.Core.Handlers;
using Newtonsoft.Json;

namespace simpsons.Core
{
    class Player : Entity
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
        public void Update(GameWindow window)
        {

            if(InputHandler.IsPressing(Keys.Left) || InputHandler.IsPressing(Keys.A))
                X -= SpeedX;
            if(InputHandler.IsPressing(Keys.Right) || InputHandler.IsPressing(Keys.D))
                X += SpeedX;
            if(InputHandler.IsPressing(Keys.Down) || InputHandler.IsPressing(Keys.S))
                Y += SpeedY;
            if(InputHandler.IsPressing(Keys.Up) || InputHandler.IsPressing(Keys.W))
                Y -= SpeedY;
            
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, vector, Color.White);
        }
    }
}