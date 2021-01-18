using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using simpsons.src.Static;
using simpsons.src.Core.Handlers;
using simpsons.src.Core;
using System.Collections.Generic;
using System;

namespace simpsons.src.Core
{
    static class Engine
    {
        public static GraphicsDevice gd;

        //Statemanagement
        public enum States {Run, Menu, Quit}
        public static States State;

        static Player player;
        

        static Menu menu;

        static Random random;
        static List<Enemy> enemies;

        //static int MaxEnemyCount = 50;
        static float value = 300;

        public static void Initialize()
        {
            random = new Random();
            enemies = new List<Enemy>();
            TextureHandler.Initialize();
            InputHandler.Initialize();
        }
        public static void LoadContent(ContentManager content, GraphicsDevice gdm, GameWindow window)
        {
            TextureHandler.LoadContent(content);
            player = new Player("Player/homer", 300, 300);
            gd = gdm;


            menu = new Menu((int)States.Menu);
            menu.LoadContent(gd, window, content);
            menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.Run, window,
                content.Load<Texture2D>("MenuIcons/Play"));
            menu.AddItem(content.Load<Texture2D>("Menu/Exit"), (int)States.Quit, window,
                content.Load<Texture2D>("MenuIcons/Exit"));
        }
        public static States RunUpdate(GameWindow window, GameTime gameTime)
        {
            foreach(Enemy e in enemies)
            {
                e.Update(gameTime, window, player);
            }
            int newBart = random.Next(1, (int)value);
            if (newBart == 1)
            {

                int rndX = random.Next(1, window.ClientBounds.Width - TextureHandler.Sprites["Enemy/bart"].Width);
                int rndspeedX = random.Next(-5, 5);
                int rndspeedY = random.Next(1, 5);
                int rndY = -30;
                if (rndspeedX != 0)
                {
                    enemies.Add(new Bart("Enemy/bart", rndX, rndY, rndspeedX, rndspeedY, 1));
                }
            }

            return States.Run;
        }
        public static void RunDraw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);
            foreach(Enemy e in enemies)
                e.Draw(spriteBatch);
        }
        public static States MenuUpdate(GameTime gameTime, GameWindow window)
        {
            MouseState mState = Mouse.GetState();
            return (States)menu.Update(gameTime, mState, window);
        }
        public static void MenuDraw(SpriteBatch spriteBatch, GameWindow window)
        {
            menu.Draw(spriteBatch, window);
        }
        public static void ExitGameSave()
        {
            SerializeGame.SerializeCurrentGame(player, enemies);
        }

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