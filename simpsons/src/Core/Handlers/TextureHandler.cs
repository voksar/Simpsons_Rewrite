using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace simpsons.Core.Handlers
{
    public static class TextureHandler
    {
        //Misc textures
        private static Texture2D backgroundTexture;

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
            Sprites.Add("MenuIcons/Saves", content.Load<Texture2D>("MenuIcons/Saves"));
            Sprites.Add("Player/homer", content.Load<Texture2D>("Player/homer"));
            Sprites.Add("Enemies/bart", content.Load<Texture2D>("Enemies/bart"));
            Sprites.Add("Player/lisa", content.Load<Texture2D>("Player/lisa"));
            
        }
    }
}