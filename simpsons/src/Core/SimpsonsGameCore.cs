using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using simpsons.src.Models;
using simpsons.src.Core;

namespace simpsons.src.Core
{
    static class Engine
    {
        //Statemanagement
        public enum States {Run, Menu, Quit}
        public static States State;

        static Player player;
        static Texture2D playerTexture;


        public static void Initialize()
        {

        }
        public static void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("Player/homer");

            player = new Player(playerTexture, 300, 300);
        }
        public static States RunUpdate()
        {
            return States.Run;
        }
        public static void RunDraw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);
        }
        public static States MenuUpdate()
        {
            return States.Menu;
        }
        public static void MenuDraw(SpriteBatch spriteBatch)
        {

        }
    }
}