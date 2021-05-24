using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using simpsons.Core.Handlers;
using simpsons.Core.Utils;
using Newtonsoft.Json;

namespace simpsons.Core
{
    public class Bullet : Entity
    {
        public Bullet(string TextureName, float X, float Y, float SpeedX, float SpeedY):base(TextureName, X, Y, SpeedX, SpeedY)
        {

        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Y += SpeedY * delta;
            X += SpeedX * delta;

            if(X < 0 || X + Texture.Width > ResolutionUtils.Width)
                IsAlive = false;
            if(Y < 0 || Y + Texture.Height > ResolutionUtils.Height)
                IsAlive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, vector, Color.White);
        }
    }
}