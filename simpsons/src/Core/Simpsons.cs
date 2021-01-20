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

        //Debugging tools
        public static bool DebuggerIsActive = true;

        //Public variables
        
        public static GraphicsDevice gd{get;set;}
        public static States State{get;set;}
        public static PlayerInformationHandler playerInformationHandler;
        
        static DisplayGames displayGames;
        static Player player;
        

        static GameHandler gameHandler;
        static List<GameHandler> gameHandlers;

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
            playerInformationHandler = PlayerInformationHandler.Initialize();

        }
        public static void LoadContent(ContentManager content, GraphicsDevice gdm, GameWindow window)
        {
            TextureHandler.LoadContent(content);
            FontHandler.LoadContent(content);

            gameHandlers = GameHandler.DeserializeOnStartup();
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
            
            enemies.Add(new Bart("Enemies/bart", 100, 30, 2,2,1));
            

            foreach(GameHandler gh in gameHandlers)
            {
                displayGames.AddGameItem(gh);
            }
        }
        public static States RunUpdate(GameWindow window, GameTime gameTime)
        {
            if(InputHandler.GoBackPressed())
            {
                StopGame();
                return States.Menu;
            }
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
            if(DebuggerIsActive)
            {
                spriteBatch.DrawString(FontHandler.Fonts["Reno20"],
                gameHandler.GameID, new Vector2(10,10), Color.Red);
            }
        }
        public static States MenuUpdate(GameTime gameTime, GameWindow window)
        {
            if(InputHandler.GoBackPressed())
                return States.Quit;
            MouseState mState = Mouse.GetState();
            return (States)menu.Update(gameTime, mState, window);
        }
        public static void MenuDraw(SpriteBatch spriteBatch, GameWindow window)
        {
            menu.Draw(spriteBatch, window);
        }
        #nullable enable
        public static States StartGame(GameHandler? gh)
        #nullable disable
        {
            if(gh == null)
            {
                gameHandler = new GameHandler();
                gameHandler.GenerateGameID();
                gameHandler.Score = 0;
            }
            else
            {
                gameHandler = gh;
                foreach(Enemy e in gameHandler.Enemies)
                    enemies.Add(e);
                player = gameHandler.Player;
                gameHandler.Score = 0;
            }
            return States.Run;
        }
        public static void StopGame()
        {
            if(gameHandler != null)
            {
                gameHandler.SetProperties(player, enemies, 5);
                displayGames.AddGameItem(gameHandler);
                gameHandlers = GameHandler.AddDataToTable(gameHandler, gameHandlers);
            }
            //gameHandler.SerializeGame(player, enemies, 0);
            gameHandler = null;
        }
        public static void ExitGame()
        {
            GameHandler.SerializeGame(gameHandlers);
        }
        public static States DisplayGamesUpdate()
        {
            if(InputHandler.GoBackPressed())
                return States.Menu;
            return (States)displayGames.Update();
        }
        public static void DisplayGamesDraw(SpriteBatch spriteBatch)
        {
            displayGames.Draw(spriteBatch);
        }
    }
}