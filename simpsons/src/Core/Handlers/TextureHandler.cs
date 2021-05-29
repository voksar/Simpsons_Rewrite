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

        private static Dictionary<string, SearchOption> _acceptablePaths = new Dictionary<string, SearchOption>()
        {
            { "Player", SearchOption.TopDirectoryOnly },
            { "Enemies", SearchOption.TopDirectoryOnly },
            { "Icons", SearchOption.TopDirectoryOnly },
            { "StoreIcons", SearchOption.TopDirectoryOnly },
            { "Menu", SearchOption.AllDirectories }
        };

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
            foreach(KeyValuePair<string, SearchOption> entry in _acceptablePaths)
            {

                //add path to next path
                //_nextPath = _currentPath + path;
                DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + $"/{entry.Key}");

                //FileInfo[] files = directory.GetFiles("*.xnb", entry.Value);
                //get all files in the accepted path
                //var files = Directory.GetFiles(_nextPath);
                //Setup settings for autoloading
                AutoLoaderSettings autoLoaderSettings = new AutoLoaderSettings()
                {
                    Path = entry.Key,
                    Content = content,
                    DirectoryInf = directory,
                    SearchOpt = entry.Value,
                    RootPath = content.RootDirectory
                };

                AutoLoaderUtils.AutoLoader(autoLoaderSettings, Sprites);
                
                
            }
        }   
    }
}