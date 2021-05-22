using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using System.Linq;
using simpsons.Core.Handlers;
using simpsons.Core.Utils;
using System.Threading;
using System.IO;

namespace simpsons.Core
{
    static class Simpsons
    {   
        //Statemanagement
        public enum States {Run, Menu, Quit, GameStart, Saves, Loading, Store}

        #region Debuggingtools
        public static bool DebuggerIsActive = true;
        public static FrameCounter frameCounter;
        public static int Tick {get; private set;}
        #endregion
        

        //Public variables and declarations
        public static GraphicsDevice graphicsDevice {get;set;}
        public static States State{get;set;}
        public static PlayerInformationHandler playerInformationHandler{get;set;}
        public static List<Enemy> Enemies {get;set;}


        static DisplayGames displayGames;
        static Player player;
        static GameHandler gameHandler;
        static Menu menu;
        static Random random;
        static Background background;
        static SpawnManager spawnManager;
        static Companion companion;
        static Store store;

        

        static List<GameHandler> gameHandlers;

        //If gamedata has been modified
        static bool NeedUpdate = false;

        //Initialization and content loading
        public static void Initialize()
        {
            //Creates folder structure if nescessary
            CreateFolderStructure();
            frameCounter = new FrameCounter();

            random = new Random();
            Enemies = new List<Enemy>();
            TextureHandler.Initialize();
            InputHandler.Initialize();
            FontHandler.Initialize();
            displayGames = new DisplayGames();
            playerInformationHandler = PlayerInformationHandler.Initialize();
            Tick = 0;
            MouseHandler.Initialize();
            spawnManager = new SpawnManager();
            store = new Store(playerInformationHandler, (int)States.Store);
        }
        public static void LoadPreContent(ContentManager content, GameWindow window, GraphicsDevice graphics)
        {
            graphicsDevice = graphics;
            //Load Pre textures and assets
            FontHandler.LoadContent(content);
            TextureHandler.LoadPreContent(content);
            background = new Background(TextureHandler.Sprites["Backgrounds/background1"], window);
        }
        public static void LoadContent(ContentManager content, GameWindow window)
        {

            TextureHandler.LoadContent(content);
            store.Load(graphicsDevice);




            //Default setup
            InitialGameSetup();
            //Deserialize all earlier games
            gameHandlers = GameHandler.DeserializeOnStartup();
            

            //Menu load stuff
            menu = new Menu((int)States.Menu);
            menu.LoadContent(graphicsDevice, window, content);
            menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.GameStart, window,
                content.Load<Texture2D>("MenuIcons/PlayNew"));
            menu.AddItem(content.Load<Texture2D>("Menu/Saves"), (int)States.Saves, window,
                content.Load<Texture2D>("MenuIcons/Saves"));
            menu.AddItem(content.Load<Texture2D>("Menu/Saves"), (int)States.Store, window,
                content.Load<Texture2D>("MenuIcons/Saves"));
            menu.AddItem(content.Load<Texture2D>("Menu/Exit"), (int)States.Quit, window,
                content.Load<Texture2D>("MenuIcons/Exit"));


            //Add previous games to the save manager
            displayGames.LoadContent(window, graphicsDevice);
            foreach(GameHandler gh in gameHandlers)
            {
                displayGames.AddGameItem(gh);
            }
        }
        
        //States and Drawers for the game
        public static States RunUpdate(GameWindow window, GameTime gameTime)
        {
            gameHandler.TimeInGame += gameTime.ElapsedGameTime.TotalSeconds;
            if(InputHandler.GoBackPressed())
            {
                StopGame();
                return States.Menu;
            }
            if(InputHandler.Press(Keys.Space))
                playerInformationHandler.Cash++;   
            player.Update(window, gameTime);
            if(companion != null)
                companion.Update();
            spawnManager.Update(Enemies, gameTime, gameHandler);
            foreach(Enemy e in Enemies.ToList())
            {
                e.Update(gameTime, window, player);
            }
            return States.Run;
        }
        public static void RunDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            player.Draw(spriteBatch);
            if(companion != null)
                companion.Draw(spriteBatch);
            foreach(Enemy e in Enemies)
                e.Draw(spriteBatch);
            if(DebuggerIsActive)
            {
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameCounter.Update(deltaTime);
                string fps = string.Format("{0}",(int)frameCounter.AverageFramesPerSecond);
                //Utilities.DrawOutlineText(spriteBatch, gameHandler.GameID + " - " + fps + 
                //" - " + (int)gameHandler.TimeInGame + " - " + Tick + " - " + Enemies.Count + " - " + playerInformationHandler.Cash);
            }
        }
        public static States MenuUpdate(GameTime gameTime, GameWindow window)
        {
            if(InputHandler.GoBackPressed())
                return States.Quit;
            return (States)menu.Update(gameTime,window);
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
                gameHandler.TimeInGame = 0;
                gameHandler.SpawnedBosses = new Dictionary<string, bool>()
                {
                    { "Maggie", false },
                    { "Wiggum", false }
                };
            }
            else
            {
                gameHandler = gameHandle;
                Enemies = gameHandler.Enemies;
                player = gameHandler.Player;
                companion = gameHandler.Companion;
            }
            return States.Run;
        }
        public static States DisplayGamesUpdate(GameTime gameTime)
        {
            return (States)displayGames.Update(gameTime);
        }
        public static void DisplayGamesDraw(SpriteBatch spriteBatch, GameWindow window)
        {
            displayGames.Draw(spriteBatch, window);
        }
        public static void AlwaysUpdate(GameWindow window, GameTime gameTime)
        
        {
            background.Update(window, 200f, gameTime);
            InputHandler.Update(gameTime);
            MouseHandler.Update();
            UpdateTick();
            if(Tick == 0)
            {
                UpdateGameSaves();
            }
        }
        public static void AlwaysDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
        }
        public static States StoreUpdate()
        {
            if(InputHandler.GoBackPressed())
                return States.Quit;
            return (States)store.Update();
        }   
        public static void StoreDraw(SpriteBatch spriteBatch)
        {
            store.Draw(spriteBatch);
        }

        //Misc functions for tasks and other nescessary stuff
        public static void UpdateTick()
        {
            Tick++;
            Tick %= 25000;
        }
        public static void UpdateGameSaves()
        {
            if(NeedUpdate)
            {
                ThreadPool.QueueUserWorkItem(state => 
                {
                    GameHandler.SerializeGame(gameHandlers);
                    NeedUpdate = false;
                });
            }
        }
        public static void StopGame()
        {
            if(gameHandler != null)
            {
                gameHandler.SetProperties(player, Enemies, 5, companion);
                gameHandlers = GameHandler.AddDataToTable(gameHandler, gameHandlers, displayGames);
                NeedUpdate = true;
            }
            gameHandler = null;
        }
        public static void ExitGame()
        {
            GameHandler.SerializeGame(gameHandlers);
            playerInformationHandler.SerializePlayerData();
        }
        public static void InitialGameSetup()
        {
            player = new Player(playerInformationHandler.SelectedPlayer, 300,300, 500,500, playerInformationHandler.SelectedBullet, 3);
            Enemies.Clear();

            if(playerInformationHandler.UnlockedCompanion)
            {
                companion = new Companion("Player\\companion", player.X + 30, player.Y + 30, 500, 500, playerInformationHandler.SelectedBullet, 5, player);
            }
        }
        public static void CreateFolderStructure()
        {
            if(!Directory.Exists("Data/"))
                Directory.CreateDirectory("Data/");
        }

        public static void RemoveGameHandler(GameHandler _gameHandler)
        {
            int index = gameHandlers.FindIndex(item => item.GameID == _gameHandler.GameID);
            if(index == -1)
            {
                Console.WriteLine("No game found");
            }
            else
            {
                gameHandlers.RemoveAt(index);
            }
        }
    }
}