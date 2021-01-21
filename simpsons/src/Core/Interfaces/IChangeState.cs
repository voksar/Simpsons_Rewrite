using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace simpsons.Core.Interfaces
{
    interface IChangeState
    {
        Texture2D Texture {get;set;}
        bool IsOpacityDone {get;set;}
        bool IsChangingState {get;set;}
        int RectangleX {get;set;}
        int RectangleWidth {get;set;}
        float Opacity {get;set;}

        public int StartStateChange(int increaseX, int increaseWidth, int targetX, int targetWidth);
    }
}