using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace simpsons.Core.Helpers
{
    static class Helper
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
    }
    
}