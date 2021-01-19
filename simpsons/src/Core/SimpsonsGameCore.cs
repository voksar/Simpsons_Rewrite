using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using simpsons.Core.Handlers;

namespace simpsons.Core
{
    static class Simpsons
    {   
        //Public variables
        public static GraphicsDevice gd;
        public static GameHandler gameHandler;
        //Statemanagement
        public enum States {Run, Menu, Quit, GameStart}
        public static States State;

        static Player player;
        

        static Menu menu;

        static Random random;
        static List<Enemy> enemies;

        //static int MaxEnemyCount = 50;
        //static float value = 300;

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
            player = new Player("Player/homer", 300, 300, 5, 5);
            gd = gdm;


            menu = new Menu((int)States.Menu);
            menu.LoadContent(gd, window, content);
            menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.GameStart, window,
                content.Load<Texture2D>("MenuIcons/Play"));
            menu.AddItem(content.Load<Texture2D>("Menu/Exit"), (int)States.Quit, window,
                content.Load<Texture2D>("MenuIcons/Exit"));
            

            enemies.Add(new Bart("Enemies/bart", 100, 100, 5, 5, 1));
            enemies.Add(new Bart("Enemies/bart", 200, 100, 5, 5, 1));
        }
        public static States RunUpdate(GameWindow window, GameTime gameTime)
        {
            foreach(Enemy e in enemies)
            {
                e.Update(gameTime, window, player);
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

        public static States StartGame()
        {
            gameHandler = new GameHandler();
            gameHandler.GenerateGameID();
            return States.Run;
        }
        public static void SerializeGame()
        {
            gameHandler.SerializeGame(player, enemies, 0);
            gameHandler.Dispose();
        }

        /*public static States CreateGame()
        {
            #nullable enable
            string? json = System.IO.File.Exists("Test.json") ? System.IO.File.ReadAllText("Test.json") : null;
            #nullable disable
            
            if(json != null)
            {
                GameInformationHandler gameInformationHandler = new GameInformationHandler();

            gameInformationHandler = JsonConvert.DeserializeObject<GameInformationHandler>(
                json, new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor}
            );
            
            foreach(Enemy e in gameInformationHandler.enemies)
                enemies.Add(e);
            player = gameInformationHandler.player;
            
            return States.Run;
            }
            return States.Menu;
        }*/

        
    }
}