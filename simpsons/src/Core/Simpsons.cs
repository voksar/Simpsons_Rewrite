using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using System.Linq;
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
            //Oklart vafan jag gör med denna, ska fixa
            gd = gdm;

            //Load textures and assets
            TextureHandler.LoadContent(content);
            FontHandler.LoadContent(content);

            //Default setup
            InitialSetup();

            //Deserialize all earlier games
            gameHandlers = GameHandler.DeserializeOnStartup();
            
            //Menu load stuff
            menu = new Menu((int)States.Menu);
            menu.LoadContent(gd, window, content);
            menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.GameStart, window,
                content.Load<Texture2D>("MenuIcons/Play"));
                menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.Saves, window,
                content.Load<Texture2D>("MenuIcons/Play"));
            menu.AddItem(content.Load<Texture2D>("Menu/Exit"), (int)States.Quit, window,
                content.Load<Texture2D>("MenuIcons/Exit"));

            //Add previous games to the save manager
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
        public static States StartGame(GameHandler gameHandle)
        {
            if(gameHandle == null)
            {
                gameHandler = new GameHandler();
                gameHandler.GenerateGameID();
                gameHandler.Score = 0;
            }
            else
            {
                gameHandler = gameHandle;
                //foreach(Enemy e in gameHandler.Enemies.ToList())
                 //   enemies.Add(e);
                 enemies = gameHandler.Enemies;
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
                gameHandlers = GameHandler.AddDataToTable(gameHandler, gameHandlers, displayGames);
            }
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
        static void InitialSetup()
        {
            player = new Player("Player/homer", 500,500, 5,5);
            enemies.Clear();
        }
    }
}