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
        const string basePath = "1123";
        public static void AutoLoader<T>(AutoLoaderSettings autoLoaderSettings, Dictionary<string, T> collection)
        {
            var subDirectories = new List<string>();

            FileInfo[] files = autoLoaderSettings.DirectoryInf.GetFiles("*.xnb", autoLoaderSettings.SearchOpt);
            foreach(FileInfo file in files)
            {
                string fileLoad = Path.GetRelativePath(autoLoaderSettings.RootPath, file.FullName)
                .Replace(@"\", "/").Replace(".xnb", "");

                collection.Add(fileLoad, autoLoaderSettings.Content.Load<T>(fileLoad));
            }

            /*foreach(FileInfo file in autoLoaderSettings.Files)
            {
                
                string key = Path.GetFileNameWithoutExtension(file.Name);
                string key_path = $"{autoLoaderSettings.Path}/{key}";
                Console.WriteLine(key_path);
                collection.Add(key_path, autoLoaderSettings.Content.Load<T>(key_path));
                

            }*/
        }
    }
    
    public class AutoLoaderSettings 
    {
        public ContentManager Content;
        public string Path;
        public DirectoryInfo DirectoryInf;
        public SearchOption SearchOpt;
        public string RootPath;
    }
}