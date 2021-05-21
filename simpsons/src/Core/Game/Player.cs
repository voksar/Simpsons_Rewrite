using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using simpsons.Core.Handlers;
using simpsons.Core.Utils;
using Newtonsoft.Json;

namespace simpsons.Core
{
    public class Player : PlayerEntity
    {
        public Player(string TextureName, float X, float Y, float SpeedX, float SpeedY, string BulletName, int Health)
         : base(TextureName, X, Y, SpeedX, SpeedY, BulletName, Health)
        {
            
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
            spriteBatch.Draw(TextureHandler.Sprites["Icons\\Border01"], new Vector2(5,5), Color.White);
            for (int i = 1; i <= HealthMax; i++)
            {
                float x = 45 + 32 * i;
                if (i <= Health)
                    spriteBatch.Draw(TextureHandler.Sprites["Player\\heart"], new Vector2(x, 5), Color.White);
                else
                    spriteBatch.Draw(TextureHandler.Sprites["Player\\heartdead"], new Vector2(x, 5), Color.White);
            }
        }
    }

    public class Companion : PlayerEntity
    {
        public Companion(string TextureName, float X, float Y, float SpeedX, float SpeedY, string BulletName, int Health)
        : base(TextureName, X, Y, SpeedX, SpeedY, BulletName, Health)
        {
        }
        public void Update()
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D borderTexture = TextureHandler.Sprites["Icons\\Border01"];
            var width = ResolutionUtils.Width;
            var height = ResolutionUtils.Height;

            var posX = width - 5 - borderTexture.Width;

            spriteBatch.Draw(Texture, vector, Color.White);
            spriteBatch.Draw(TextureHandler.Sprites["Icons\\Border01"], new Vector2(posX,5), Color.White);
            for (int i = 1; i <= HealthMax; i++)
            {
                float x = (posX - 5) - 32 * i;
                if (i <= Health)
                    spriteBatch.Draw(TextureHandler.Sprites["Player\\heart"], new Vector2(x, 5), Color.White);
                else
                    spriteBatch.Draw(TextureHandler.Sprites["Player\\heartdead"], new Vector2(x, 5), Color.White);
            }
        }
    }
}