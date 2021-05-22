using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace simpsons.Core.Interfaces
{
    interface IState
    {
        Texture2D Texture {get;set;}
        bool IsOpacityDone {get;set;}
        bool IsChangingState {get;set;}
        float RectangleX {get;set;}
        float RectangleWidth {get;set;}
        float Opacity {get;set;}

        public int StartStateChange(int changeX, int changeWidth, int targetX, int targetWidth, GameTime gameTime);
        public void LoadStateChangeVariables();
    }
}