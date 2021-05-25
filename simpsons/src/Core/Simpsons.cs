using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static ObservableCollection<Enemy> Enemies {get;set;}


        static DisplayGames displayGames;
        static Player player;
        static GameHandler gameHandler;
        static Menu menu;
        static Random random;
        static Background background;
        static SpawnManager spawnManager;
        static Companion companion;
        static Store store;

        //Loading
        private static string loading = "Loading";
        private const int maxDots = 5;
        private static int dots = 0;
        private static float duration = 0.3f;
        private static bool loaded = false;

        static List<GameHandler> gameHandlers;

        //If gamedata has been modified
        private static bool _needUpdate = false;
        public static bool NeedUpdate
        {
            get { return _needUpdate; }
            set
            {
                _needUpdate = value;
                if(_needUpdate)
                {
                    Console.WriteLine("Game serialization ran");
                    ThreadPool.QueueUserWorkItem(state => 
                    {
                        GameHandler.SerializeGame(gameHandlers);
                        playerInformationHandler.SerializePlayerData();
                        _needUpdate = false;
                    });
                }
            }
        }

        //Initialization and content loading
        public static void Initialize()
        {
            //Creates folder structure if nescessary
            CreateFolderStructure();
            frameCounter = new FrameCounter();

            random = new Random();
            
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
            




            //Deserialize all earlier games
            gameHandlers = GameHandler.DeserializeOnStartup();
            store.Load(graphicsDevice);

            //Menu load stuff
            menu = new Menu((int)States.Menu);
            menu.LoadContent(graphicsDevice, window, content);
            menu.AddItem(content.Load<Texture2D>("Menu/Play"), (int)States.GameStart, window,
                content.Load<Texture2D>("MenuIcons/PlayNew"));
            menu.AddItem(content.Load<Texture2D>("Menu/Store"), (int)States.Saves, window,
                content.Load<Texture2D>("MenuIcons/Saves"));
            menu.AddItem(content.Load<Texture2D>("Menu/Store"), (int)States.Store, window,
                content.Load<Texture2D>("MenuIcons/Collection"));
            menu.AddItem(content.Load<Texture2D>("Menu/Exit"), (int)States.Quit, window,
                content.Load<Texture2D>("MenuIcons/Exit"));


            //check progress on eg. dog unlock etc
            CheckProgress();

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

            player.Update(window, gameTime);

            if(companion != null && companion.IsAlive)
                companion.Update(gameTime, Enemies);

            spawnManager.Update(Enemies, gameTime, gameHandler);

            foreach(Enemy e in Enemies.ToList())
            {
                foreach(Bullet bullet in player.Bullets.ToList())
                {
                    if(bullet.CheckCollision(e))
                    {
                        e.Health -= playerInformationHandler.Damage;
                        player.Bullets.Remove(bullet);
                    }
                }
                if(companion != null)
                {
                    foreach(Bullet bullet in companion.Bullets.ToList())
                    {
                        if(bullet.CheckCollision(e))
                        {
                            e.Health -= playerInformationHandler.Damage;
                            companion.Bullets.Remove(bullet);
                        }
                    }
                    if(e.CheckCollision(companion))
                    {
                        companion.Health -= e.Health;
                        e.Health = 0;
                    }
                }
                if(e.CheckCollision(player))
                {
                    player.Health -= e.Health;
                    e.Health = 0;
                }
                
                
                    
                e.Update(gameTime, window, player);

                if(!e.IsAlive)
                    Enemies.Remove(e);
            }

            if(!player.IsAlive)
                return GameOver();
                
            return States.Run;
        }
        public static void RunDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            player.Draw(spriteBatch);
            if(companion != null && companion.IsAlive)
                companion.Draw(spriteBatch);
            foreach(Enemy e in Enemies)
                e.Draw(spriteBatch);

            float x_cash = 5;
            float y_cash = 10 + TextureHandler.Sprites["Icons/Border01"].Height;

            spriteBatch.DrawString(FontHandler.Fonts["Fonts/Reno14"], gameHandler.Score + "$", new Vector2(x_cash, y_cash), Color.White);
        }
        public static States MenuUpdate(GameTime gameTime, GameWindow window)
        {
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
                gameHandler = GameHandler.GenerateHandler();
                Enemies = gameHandler.Enemies;
                playerInformationHandler.VerifyUnlockedPlayer();
                Reload();
                if(playerInformationHandler.UnlockedCompanion)
                    companion = new Companion("Player/companion", player.X + 30, player.Y + 30, 200, 200, playerInformationHandler.SelectedBullet, 5, player);
            }
            else
            {
                gameHandler = gameHandle;
                Enemies = gameHandler.Enemies;
                player = gameHandler.Player;
                companion = gameHandler.Companion;
            }

            Enemies.CollectionChanged += EnemiesChanged;
            return States.Run;
        }
        public static void StopGame()
        {
            if(gameHandler != null)
            {
                gameHandler.SetProperties(player, Enemies, companion);
                gameHandlers = GameHandler.AddDataToTable(gameHandler, gameHandlers, displayGames);
                NeedUpdate = true;
            }
            
            gameHandler = null;
        }
        public static States GameOver()
        {
            return States.Menu;
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

            if(DebuggerIsActive)
            {
                double x = 2;
                x = Math.Pow(x, 20);
                int usage = ((int)GC.GetTotalMemory(false) / (int)x);
                window.Title = $"Simpsons - Development - Build (0.2.0) - {frameCounter.FormattedFPS()} - {usage}MB";
            }
            
        }
        public static void AlwaysDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
        }
        public static States StoreUpdate(GameTime gameTime)
        {
            return (States)store.Update(gameTime);
        }   
        public static void StoreDraw(SpriteBatch spriteBatch)
        {
            store.Draw(spriteBatch);
        }

        public static States LoadingUpdate(GameTime gameTime, bool isLoaded)
        {
            loaded = isLoaded;
            if(!loaded)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                duration -= delta;
                if(duration <= 0)
                {
                    duration = 0.3f;
                    dots++;
                }
                dots %= maxDots;
            }
            

            return States.Loading;
        }
        public static void LoadingDraw(SpriteBatch spriteBatch)
        {
            if(!loaded)
            {
                string finalLoadingString = loading;
                for(int i = 0; i <= dots; i++)
                    finalLoadingString = finalLoadingString + " " +  ".";
            
                spriteBatch.DrawString(FontHandler.Fonts["Fonts/Reno20"], finalLoadingString, new Vector2(100,100), Color.Red);
            }
            else if(loaded)
            {
                spriteBatch.DrawString(FontHandler.Fonts["Fonts/Reno20"], "Done, press any key to continue", new Vector2(100,100), Color.Red);
            }
            
        }
        //Misc functions for tasks and other nescessary stuff
        public static void UpdateTick()
        {
            Tick++;
            Tick %= 25000;
        }

        public static void ExitGame()
        {
            GameHandler.SerializeGame(gameHandlers);
            playerInformationHandler.SerializePlayerData();
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

        public static void Reload()
        {
            player = new Player(playerInformationHandler.SelectedPlayer, 300,300, 420,420, playerInformationHandler.SelectedBullet, 3);
        }

        private static void CheckProgress()
        {
            if(playerInformationHandler.TotalKills >= 25000 & !playerInformationHandler.UnlockedCompanion)
                playerInformationHandler.UnlockedCompanion = true;
                NeedUpdate = true;
        }
        private static void EnemiesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                playerInformationHandler.TotalKills++;
                playerInformationHandler.Cash++;
                gameHandler.Score++;
                CheckProgress();
            }
        }
    }
}