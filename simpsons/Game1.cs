using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simpsons.Core.Handlers;
using simpsons.Core;

namespace simpsons
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();
            //Intialize
            Simpsons.State = Simpsons.States.Menu;
            Simpsons.Initialize();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Simpsons.LoadContent(Content, GraphicsDevice, Window);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Simpsons.State = Simpsons.States.Quit;*/
            InputHandler.Update(gameTime);
            switch(Simpsons.State)
            {
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

            _spriteBatch.Begin();
            switch(Simpsons.State)
            {
                case Simpsons.States.Saves:
                    Simpsons.DisplayGamesDraw(_spriteBatch);
                    break;
                case Simpsons.States.Run:
                    Simpsons.RunDraw(_spriteBatch);
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
