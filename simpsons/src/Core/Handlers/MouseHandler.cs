using Microsoft.Xna.Framework.Input;

namespace simpsons.Core.Handlers
{
    static class MouseHandler
    {
        public static MouseState MouseState {get;set;}

        private static MouseState previousMouseState;

        public static void Initialize()
        {
            previousMouseState = MouseState;
            MouseState = new MouseState();
        }

        public static void Update()
        {
            previousMouseState = MouseState;
            MouseState = Mouse.GetState();
        }
        public static bool CheckIfSameSpot()
        {
            return MouseState.Position == previousMouseState.Position;
        }

    }
}