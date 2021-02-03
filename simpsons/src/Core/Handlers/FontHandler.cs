using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace simpsons.Core.Handlers
{
    static class FontHandler
    {
        public static Dictionary<string, SpriteFont> Fonts {get; private set;}

        static SpriteFont reno14;
        static SpriteFont reno20;
        static SpriteFont reno24;

        static SpriteFont vcr16;

        public static void Initialize()
        {
            Fonts = new Dictionary<string, SpriteFont>();
        }

        public static void LoadContent(ContentManager content)
        {
            reno14 = content.Load<SpriteFont>("Fonts/Reno14");
            reno20 = content.Load<SpriteFont>("Fonts/Reno20");
            reno24 = content.Load<SpriteFont>("Fonts/Reno24");
            
            vcr16 = content.Load<SpriteFont>("Fonts/VCR16");

            Fonts.Add("Reno14", reno14);
            Fonts.Add("Reno20", reno20);
            Fonts.Add("Reno24", reno24);

            Fonts.Add("VCR16", vcr16);
        }
    }
}