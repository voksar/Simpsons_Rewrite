using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace simpsons.Core.Handlers
{
    public static class TextureHandler
    {
        //Player related textures
        private static Texture2D playerTexture;

        //Enemy related pictures
        private static Texture2D enemyBartTexture;


        //Misc textures
        private static Texture2D backgroundTexture;
        private static Texture2D saveIcon;

        public static Dictionary<string, Texture2D> Sprites{get; private set;}

        public static void Initialize()
        {
            Sprites = new Dictionary<string, Texture2D>();
        }
        public static void LoadPreContent(ContentManager content)
        {
            backgroundTexture = content.Load<Texture2D>("Backgrounds/background1");
            Sprites.Add("Backgrounds/background1", backgroundTexture);
        }
        public static void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("Player/homer");
            enemyBartTexture = content.Load<Texture2D>("Enemies/bart");
            saveIcon = content.Load<Texture2D>("MenuIcons/Saves");
            
            Sprites.Add("MenuIcons/Saves", saveIcon);
            Sprites.Add("Player/homer", playerTexture);
            Sprites.Add("Enemies/bart", enemyBartTexture);
            
        }
    }
}