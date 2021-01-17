using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
namespace simpsons.src.Core.Handlers
{
    static class InputHandler
    {
        public static KeyboardState prevState, currentState;

        public static void Initialize()
        {
            prevState = currentState;
            currentState = Keyboard.GetState();
        }

        public static void Update(GameTime gameTime)
        {
            prevState = currentState;
            currentState = Keyboard.GetState();
        }
        
        public static bool Press(Keys key)
        {
            return currentState.IsKeyDown(key) && !prevState.IsKeyDown(key);
        }
    }
}