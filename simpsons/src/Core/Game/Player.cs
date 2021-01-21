using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using simpsons.Core.Handlers;

namespace simpsons.Core
{
    class Player : Entity
    {
        public Player(string TextureName, float X, float Y, float SpeedX, float SpeedY)
         : base(TextureName, X, Y, SpeedX, SpeedY)
        {

        }
        public void Update()
        {
            if(InputHandler.IsPressing(Keys.Down) || InputHandler.Press(Keys.S))
                Y += SpeedY;
            if(InputHandler.IsPressing(Keys.Up) || InputHandler.Press(Keys.W))
                Y -= SpeedY;
            if(InputHandler.IsPressing(Keys.Left) || InputHandler.Press(Keys.A))
                X -= SpeedX;
            if(InputHandler.IsPressing(Keys.Right) || InputHandler.Press(Keys.D))
                X += SpeedX;
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, vector, Color.White);
        }
    }
}