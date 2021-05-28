using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using simpsons.Core.Handlers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using System.IO;

namespace simpsons.Core.Utils
{
    static class AutoLoaderUtils
    {
        public static void AutoLoader<T>(AutoLoaderSettings autoLoaderSettings, Dictionary<string, T> collection)
        {
            foreach(FileInfo file in autoLoaderSettings.Files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                string key_path = $"{autoLoaderSettings.Path}/{key}";
                collection.Add(key_path, autoLoaderSettings.Content.Load<T>(key_path));

            }
        }
    }
    
    public class AutoLoaderSettings 
    {
        public ContentManager Content;
        public string Path;
        public FileInfo[] Files;
    }
}