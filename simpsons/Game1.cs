using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using simpsons.src.Core;

namespace simpsons
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Intialize
            Engine.State = Engine.States.Run;
            Engine.Initialize();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Engine.LoadContent(Content);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch(Engine.State)
            {
                case Engine.States.Run:
                    Engine.RunUpdate();
                    break;
                case Engine.States.Quit:
                    Exit();
                    break;
                case Engine.States.Menu:
                    Engine.MenuUpdate();
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
                    Engine.MenuDraw(_spriteBatch);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
