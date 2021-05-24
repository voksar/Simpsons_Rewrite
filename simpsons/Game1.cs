using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simpsons.Core.Handlers;
using simpsons.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using simpsons.Core.Utils;

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
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 144);
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            //Intialize
            ResolutionUtils.SetResolution(1600, 900);
            Simpsons.State = Simpsons.States.Loading;
            Simpsons.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Simpsons.LoadPreContent(Content, Window, GraphicsDevice);
            //Start up new thread to not block Draw thread
            ThreadPool.QueueUserWorkItem(state => 
            {
                Simpsons.LoadContent(Content, Window);
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
                    Simpsons.State = Simpsons.DisplayGamesUpdate(gameTime);
                    break;
                case Simpsons.States.Quit:
                    Simpsons.ExitGame();
                    Exit();
                    break;
                case Simpsons.States.Store:
                    Simpsons.State = Simpsons.StoreUpdate(gameTime);
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
                case Simpsons.States.Store:
                    Simpsons.StoreDraw(_spriteBatch);
                    break;
            }
            _spriteBatch.DrawString(FontHandler.Fonts["Fonts\\Reno14"], "Simpsons(v0.2.0)", new Vector2(0, 880), Color.White * 0.5f);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
