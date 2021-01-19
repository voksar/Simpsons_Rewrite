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
        //Statemanagement
        public enum States {Run, Menu, Quit, GameStart, Saves}


        //Public variables
        public static GraphicsDevice gd{get;set;}
        public static GameHandler gameHandler{get;set;}
        public static States State{get;set;}
        
        
        static DisplayGames displayGames;
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
            FontHandler.Initialize();
            displayGames = new DisplayGames();
        }
        public static void LoadContent(ContentManager content, GraphicsDevice gdm, GameWindow window)
        {
            TextureHandler.LoadContent(content);
            FontHandler.LoadContent(content);
            player = new Player("Player/homer", 300, 300, 5, 5);
            gd = gdm;


            menu = new Menu((int)States.Menu);
            menu.LoadContent(gd, window, content);
            menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.GameStart, window,
                content.Load<Texture2D>("MenuIcons/Play"));
                menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.Saves, window,
                content.Load<Texture2D>("MenuIcons/Play"));
            menu.AddItem(content.Load<Texture2D>("Menu/Exit"), (int)States.Quit, window,
                content.Load<Texture2D>("MenuIcons/Exit"));
            

            enemies.Add(new Bart("Enemies/bart", 100, 100, 5, 5, 1));
            enemies.Add(new Bart("Enemies/bart", 200, 100, 5, 5, 1));
            
            displayGames.AddGameItem("TEST123");
            displayGames.AddGameItem("TEST321");

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

        public static States DisplayGamesUpdate()
        {
            displayGames.Update();
            return States.Saves;
        }
        public static void DisplayGamesDraw(SpriteBatch spriteBatch)
        {
            displayGames.Draw(spriteBatch);
        }
    }
}