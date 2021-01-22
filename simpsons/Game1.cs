using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simpsons.Core.Handlers;
using simpsons.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using simpsons.Core.Helpers;

namespace simpsons
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        
        private bool isLoaded = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.Title = "Simpsons";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1000;
            //Ta bort v-sync, cappa på 60 fps.
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            //Intialize
            Simpsons.State = Simpsons.States.Loading;
            Simpsons.Initialize();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Simpsons.LoadPreContent(Content, Window);
            //Start up new thread to not block Draw thread
            ThreadPool.QueueUserWorkItem(state => 
            {
                Simpsons.LoadContent(Content, GraphicsDevice, Window);
                isLoaded = true;
            });
        }

        protected override void Update(GameTime gameTime)
        {
            Simpsons.AlwaysUpdate(Window, gameTime);
            switch(Simpsons.State)
            {
                case Simpsons.States.Loading:
                    if(isLoaded)
                        if(InputHandler.AnyPressed())
                            Simpsons.State = Simpsons.States.Menu;
                    break;
                case Simpsons.States.Run:
                    Simpsons.State = Simpsons.RunUpdate(Window, gameTime);
                    break;
                case Simpsons.States.GameStart:
                    Simpsons.State = Simpsons.StartGame(null);
                    break;
                case Simpsons.States.Saves:
                    Simpsons.State = Simpsons.DisplayGamesUpdate();
                    break;
                case Simpsons.States.Quit:
                    Simpsons.ExitGame();
                    Exit();
                    break;
                case Simpsons.States.Menu:
                    Simpsons.State = Simpsons.MenuUpdate(gameTime, Window);
                    break;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
            SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            Simpsons.AlwaysDraw(_spriteBatch);
            switch(Simpsons.State)
            {
                case Simpsons.States.Loading:
                    if(isLoaded)
                        Helper.DrawOutlineText(_spriteBatch, "Loading Done, press any key to continue");
                    else
                        Helper.DrawOutlineText(_spriteBatch, "Loading Content");
                    break;
                case Simpsons.States.Saves:
                    Simpsons.DisplayGamesDraw(_spriteBatch, Window);
                    break;
                case Simpsons.States.Run:
                    Simpsons.RunDraw(_spriteBatch, gameTime);
                    break;
                case Simpsons.States.Menu:
                    Simpsons.MenuDraw(_spriteBatch, Window);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
