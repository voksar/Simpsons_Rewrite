using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using simpsons.Core.Handlers;
using simpsons.Core.Helpers;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace simpsons.Core
{
    static class Simpsons
    {   
        //Statemanagement
        public enum States {Run, Menu, Quit, GameStart, Saves, Loading}

        //Debugging tools
        public static bool DebuggerIsActive = true;
        public static FrameCounter frameCounter;
        public static int Tick {get; private set;}
        

        //Public variables
        public static GraphicsDevice graphicsDevice{get;set;}
        public static States State{get;set;}
        public static PlayerInformationHandler playerInformationHandler;
        
        static DisplayGames displayGames;
        static Player player;
        static GameHandler gameHandler;
        static Menu menu;
        static Random random;
        static Background background;

        static List<Enemy> enemies;
        static List<GameHandler> gameHandlers;


        static bool NeedUpdate = false;

        public static void Initialize()
        {
            CreateFolderStructure();
            frameCounter = new FrameCounter();

            random = new Random();
            enemies = new List<Enemy>();
            TextureHandler.Initialize();
            InputHandler.Initialize();
            FontHandler.Initialize();
            displayGames = new DisplayGames();
            playerInformationHandler = PlayerInformationHandler.Initialize();
            Tick = 0;
            MouseHandler.Initialize();
        }
        public static void LoadPreContent(ContentManager content, GameWindow window)
        {
            //Load Pre textures and assets
            FontHandler.LoadContent(content);
            TextureHandler.LoadPreContent(content);
            background = new Background(TextureHandler.Sprites["Backgrounds/background1"], window);
        }
        public static void LoadContent(ContentManager content, GraphicsDevice gdm, GameWindow window)
        {
            //Oklart vafan jag gör med denna, ska fixa
            graphicsDevice = gdm;

            TextureHandler.LoadContent(content);
            

            //Default setup
            InitialGameSetup();

            //Deserialize all earlier games
            gameHandlers = GameHandler.DeserializeOnStartup();
            
            //Menu load stuff
            menu = new Menu((int)States.Menu);
            menu.LoadContent(graphicsDevice, window, content);
            menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.GameStart, window,
                content.Load<Texture2D>("MenuIcons/PlayNew"));
                menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.Saves, window,
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
        public static States RunUpdate(GameWindow window, GameTime gameTime)
        {
            
            gameHandler.TimeInGame += gameTime.ElapsedGameTime.TotalSeconds;
            if(InputHandler.GoBackPressed())
            {
                StopGame();
                return States.Menu;
            }
            player.Update(window);
            foreach(Enemy e in enemies)
            {
                e.Update(gameTime, window, player);
            }
            return States.Run;
        }
        public static void RunDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            player.Draw(spriteBatch);
            foreach(Enemy e in enemies)
                e.Draw(spriteBatch);
            if(DebuggerIsActive)
            {
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameCounter.Update(deltaTime);
                string fps = string.Format("{0}",(int)frameCounter.AverageFramesPerSecond);
                Helper.DrawOutlineText(spriteBatch, gameHandler.GameID + " - " + fps + 
                " - " + (int)gameHandler.TimeInGame + " - " + Tick);
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
            }
            else
            {
                gameHandler = gameHandle;
                enemies = gameHandler.Enemies;
                player = gameHandler.Player;
            }
            return States.Run;
        }
        public static States DisplayGamesUpdate()
        {
            return (States)displayGames.Update();
        }
        public static void DisplayGamesDraw(SpriteBatch spriteBatch, GameWindow window)
        {
            displayGames.Draw(spriteBatch, window);
        }
        public static void AlwaysUpdate(GameWindow window, GameTime gameTime)
        {
            background.Update(window, 2f);
            InputHandler.Update(gameTime);
            MouseHandler.Update();
            UpdateTick();
            if(Tick == 900)
            {
                UpdateGameSaves();
            }
        }
        public static void AlwaysDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
        }

        public static void UpdateTick()
        {
            Tick++;
            Tick %= 1800;
        }
        public static void UpdateGameSaves()
        {
            if(NeedUpdate)
            {
                ThreadPool.QueueUserWorkItem(state => 
                {
                    Console.WriteLine("Updating the saves");
                    GameHandler.SerializeGame(gameHandlers);
                    NeedUpdate = false;
                });
            }
        }
        public static void StopGame()
        {
            if(gameHandler != null)
            {
                gameHandler.SetProperties(player, enemies, 5);
                gameHandlers = GameHandler.AddDataToTable(gameHandler, gameHandlers, displayGames);
                NeedUpdate = true;
            }
            gameHandler = null;
        }
        public static void ExitGame()
        {
            GameHandler.SerializeGame(gameHandlers);
        }
        static void InitialGameSetup()
        {
            player = new Player("Player/homer", 500,500, 5,5, "Player/homer");
            enemies.Clear();
        }
    
        public static void CreateFolderStructure()
        {
            if(!Directory.Exists("Data/"))
                Directory.CreateDirectory("Data/");
            if(!Directory.Exists("Saves/"))
                Directory.CreateDirectory("Saves/");
        }
    }
}