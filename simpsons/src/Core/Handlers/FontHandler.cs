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
    static class FontHandler
    {
        public static Dictionary<string, SpriteFont> Fonts {get; private set;}


        private static Dictionary<string, SearchOption> _acceptablePaths = new Dictionary<string, SearchOption>()
        {
            { "Fonts", SearchOption.TopDirectoryOnly }
        };

        public static void Initialize()
        {
            Fonts = new Dictionary<string, SpriteFont>();
        }

        public static void LoadContent(ContentManager content)
        {
                        //Auto-loader for any textures located in _acceptablePaths folder
            foreach(KeyValuePair<string, SearchOption> entry in _acceptablePaths)
            {
                //get all files in the accepted path
                //var files = Directory.GetFiles(_nextPath);
                DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + $"/{entry.Key}");


                //FileInfo[] files = directory.GetFiles("*.xnb");
                //Setup settings for autoloading
                AutoLoaderSettings autoLoaderSettings = new AutoLoaderSettings()
                {
                    Path = entry.Key,
                    Content = content,
                    DirectoryInf = directory,
                    SearchOpt = entry.Value,
                    RootPath = content.RootDirectory
                };

                AutoLoaderUtils.AutoLoader(autoLoaderSettings, Fonts);
                
                
            }
        }
    }
}