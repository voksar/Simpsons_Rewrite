using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace simpsons.Core
{
    class BackgroundSprite
    {
        public float X {get;set;}
        public float Y {get;set;}
        public Texture2D Texture {get;set;}
        public BackgroundSprite(Texture2D Texture, float X, float Y)
        {
            this.Texture = Texture;
            this.X = X;
            this.Y = Y;
        }
        public void Update(GameWindow window, int nrBackgroundsY, float speed)
        {
            Y += speed;
            if (Y > window.ClientBounds.Height)
            {
                Y = Y - nrBackgroundsY * Texture.Height;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), Color.White);
        }
    }
    class Background
    {
        BackgroundSprite[,] background;
        int nrBackgroundsX, nrBackgroundsY;

        public Texture2D Texture;

        public Background(Texture2D Texture, GameWindow window)
        {
            this.Texture = Texture;
            double tmpX = (double)Texture.Width;//window.ClientBounds.Width / texture.Width;

            nrBackgroundsX = (int)Math.Ceiling(tmpX);

            double tmpY = (double)window.ClientBounds.Height / Texture.Height;

            nrBackgroundsY = (int)Math.Ceiling(tmpY) + 1;

            background = new BackgroundSprite[nrBackgroundsX, nrBackgroundsY];
            
            for (int i = 0; i < nrBackgroundsX; i++)
            {
                for (int j = 0; j < nrBackgroundsY; j++)
                {
                    int posX = i * Texture.Width;
                    int posY = j * Texture.Height - Texture.Height;
                    
                    background[i, j] = new BackgroundSprite(Texture, posX, posY);
                }
            }
        }
        public void Update(GameWindow window, float speed)
        {
            
            for (int i = 0; i < nrBackgroundsX; i++)
                for (int j = 0; j < nrBackgroundsY; j++)
                    background[i, j].Update(window, nrBackgroundsY, speed);
                    
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < nrBackgroundsX; i++)
                for (int j = 0; j < nrBackgroundsY; j++)
                    background[i, j].Draw(spriteBatch);
        }
    }
}