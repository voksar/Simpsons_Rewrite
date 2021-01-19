using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, vector, Color.White);
        }
    }
}