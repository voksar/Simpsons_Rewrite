using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using simpsons.Core.Utils;
using System;

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
            "Icons",
            "StoreIcons"
        };

        /*private static List <string> _acceptableExtensions = new List<string>()
        {
            ".jpg",
            ".png",
            ".xnb"
        };*/

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
            //Auto-loader for any textures located in _acceptablePaths folder
            foreach(string path in _acceptablePaths)
            {

                //add path to next path
                //_nextPath = _currentPath + path;
                DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + $"/{path}");

                FileInfo[] files = directory.GetFiles("*.xnb");
                //get all files in the accepted path
                //var files = Directory.GetFiles(_nextPath);
                
                //Setup settings for autoloading
                AutoLoaderSettings autoLoaderSettings = new AutoLoaderSettings()
                {
                    Path = path,
                    Content = content,
                    Files = files,
                };

                AutoLoaderUtils.AutoLoader(autoLoaderSettings, Sprites);
                
                
            }
        }   
    }
}