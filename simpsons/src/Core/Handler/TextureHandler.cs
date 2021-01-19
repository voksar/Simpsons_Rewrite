using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace simpsons
{
    public static class TextureHandler
    {
        private static Texture2D playerTexture;
        private static Texture2D enemyBart;

        public static Dictionary<string, Texture2D> Sprites{get; private set;}

        public static void Initialize()
        {
            Sprites = new Dictionary<string, Texture2D>();
        }

        public static void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("Player/homer");
            enemyBart = content.Load<Texture2D>("Enemies/bart");

            Sprites.Add("Player/homer", playerTexture);
            Sprites.Add("Enemy/bart", enemyBart);
        }
    }
}