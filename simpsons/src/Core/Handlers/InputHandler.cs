using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
namespace simpsons.Core.Handlers
{
    static class InputHandler
    {
        static KeyboardState previousState, currentState;

        public static void Initialize()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
        }

        public static void Update(GameTime gameTime)
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
        }
        public static bool GoBackPressed()
        {
            return Press(Keys.Back)
                || Press(Keys.Escape);
        }
        public static bool Press(Keys key)
        {
            return currentState.IsKeyDown(key) && !previousState.IsKeyDown(key);
        }
    }
}