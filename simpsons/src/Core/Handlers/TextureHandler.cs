using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using simpsons.Core.Utils;


namespace simpsons.Core.Handlers
{
    public static class TextureHandler
    {
        //Misc textures
        private static Texture2D backgroundTexture;

        public static Dictionary<string, Texture2D> Sprites{get; private set;}

        private static List<string> _acceptablePaths = new List<string>()
        {
            "MenuIcons",
            "Player",
            "Enemies",
            "Icons"
        };

        private static List <string> _acceptableExtensions = new List<string>()
        {
            ".jpg",
            ".png"
        };

        private static string _currentPath;

        private static string _nextPath;


        public static void Initialize()
        {
            Sprites = new Dictionary<string, Texture2D>();
            _currentPath = Directory.GetCurrentDirectory() + "\\Content\\";
        }
        public static void LoadPreContent(ContentManager content)
        {
            backgroundTexture = content.Load<Texture2D>("Backgrounds/background1");
            Sprites.Add("Backgrounds/background1", backgroundTexture);
        }
        public static void LoadContent(ContentManager content)
        {
            //Auto-loader for any textures located in _acceptablePaths folder
            foreach(string path in _acceptablePaths)
            {

                //add path to next path
                _nextPath = _currentPath + path;
                
                

                //get all files in the accepted path
                var files = Directory.GetFiles(_nextPath);
                
                //Setup settings for autoloading
                AutoLoaderSettings autoLoaderSettings = new AutoLoaderSettings()
                {
                    ReplacePath = _currentPath,
                    AcceptableExtensions = _acceptableExtensions,
                    Content = content,
                    Files = files,
                };

                Utilities.AutoLoader(autoLoaderSettings, Sprites);
                
                
            }
        }   
    }
}