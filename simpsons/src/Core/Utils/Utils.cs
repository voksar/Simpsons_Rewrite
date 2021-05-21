using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using simpsons.Core.Handlers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace simpsons.Core.Utils
{
    static class Utilities
    {
        public static Texture2D RectangleCreator(int dim1, int dim2, GraphicsDevice gd, Color c)
        {
            Texture2D rect = new Texture2D(gd, dim1, dim2);
            Color[] data = new Color[dim1 * dim2];
            for (int i = 0; i < data.Length; ++i) data[i] = c;
            rect.SetData(data);
            return rect;
        }
        public static Texture2D RectangleCreator(int dim1, int dim2, GraphicsDevice gd, Color c, float opacity)
        {
            Texture2D rect = new Texture2D(gd, dim1, dim2);
            Color[] data = new Color[dim1 * dim2];
            for (int i = 0; i < data.Length; ++i) data[i] = c * opacity;
            rect.SetData(data);
            return rect;
        }
        //Högst icke optimerat, skall försöka bygga en shader för att få outlines på texten.
        public static void DrawOutlineText(SpriteBatch spriteBatch, string text)
        {
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(9,10), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(9,9), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(10,9), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(11,10), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(11,11), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(10,11), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(10,10), Color.White);
        }
        public static void DrawOutlineText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X - 1, position.Y), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X - 1, position.Y - 1), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X, position.Y - 1), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X + 1, position.Y), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X + 1, position.Y + 1), Color.Black);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X, position.Y + 1), Color.Black);

            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, position, color);
        }
        public static void DrawOutlineText(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float opacity)
        {
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X - 1, position.Y), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X - 1, position.Y - 1), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X, position.Y - 1), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X + 1, position.Y), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X + 1, position.Y + 1), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, new Vector2(position.X, position.Y + 1), Color.Black * opacity);

            spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
            text, position, color * opacity);
        }
        public static void DrawOutlineText(string font,SpriteBatch spriteBatch, string text, Vector2 position, Color color, float opacity)
        {
            spriteBatch.DrawString(FontHandler.Fonts[font],
            text, new Vector2(position.X - 1, position.Y), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts[font],
            text, new Vector2(position.X - 1, position.Y - 1), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts[font],
            text, new Vector2(position.X, position.Y - 1), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts[font],
            text, new Vector2(position.X + 1, position.Y), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts[font],
            text, new Vector2(position.X + 1, position.Y + 1), Color.Black * opacity);
            spriteBatch.DrawString(FontHandler.Fonts[font],
            text, new Vector2(position.X, position.Y + 1), Color.Black * opacity);

            spriteBatch.DrawString(FontHandler.Fonts[font],
            text, position, color * opacity);
        }
    
        public static void AutoLoader<T>(AutoLoaderSettings autoLoaderSettings, Dictionary<string, T> collection)
        {
            string _relativePath;
            var toBeLoadedFiles = autoLoaderSettings.Files.Where(s => autoLoaderSettings.AcceptableExtensions.Any(x => s.Contains(x)));

            foreach(string file in toBeLoadedFiles)
            {
                _relativePath = file;
                autoLoaderSettings.AcceptableExtensions.ForEach(x => _relativePath = _relativePath.Replace(x, "").Replace(autoLoaderSettings.ReplacePath, ""));
                collection.Add(_relativePath, autoLoaderSettings.Content.Load<T>(_relativePath));
            }
        }
    }
    
    public class AutoLoaderSettings 
    {
        public string ReplacePath;
        public ContentManager Content;
        public List<string> AcceptableExtensions;
        public string[] Files;
    }
}