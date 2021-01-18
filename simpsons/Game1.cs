using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simpsons.src.Core;
using simpsons.src.Core.Handlers;

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
            Engine.State = Engine.States.Menu;
            Engine.Initialize();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Engine.LoadContent(Content, GraphicsDevice, Window);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Engine.State = Engine.States.Quit;
            InputHandler.Update(gameTime);
            switch(Engine.State)
            {
                case Engine.States.Run:
                    Engine.State = Engine.RunUpdate(Window, gameTime);
                    break;
                case Engine.States.Quit:
                    Engine.ExitGameSave();
                    Exit();
                    break;
                case Engine.States.Menu:
                    Engine.State = Engine.MenuUpdate(gameTime, Window);
                    break;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            switch(Engine.State)
            {
                case Engine.States.Run:
                    Engine.RunDraw(_spriteBatch);
                    break;
                case Engine.States.Menu:
                    Engine.MenuDraw(_spriteBatch, Window);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
