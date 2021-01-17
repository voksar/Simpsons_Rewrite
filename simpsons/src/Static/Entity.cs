using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Newtonsoft.Json;

namespace simpsons.src.Static
{
    [JsonObject(MemberSerialization.OptIn)]
    class Entity
    {
        protected Vector2 vector;
        protected Vector2 speed;
        public Entity(Texture2D texture, float x, float y, float speedX, float speedY)
        {
            Texture = texture;
            speed.X = speedX;
            speed.Y = speedY;
            vector.X = x;
            vector.Y = y;
            IsAlive = true;
            TextureName = Texture.ToString();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, vector, Color.White);
        }
        public Texture2D Texture {get;set;}
        [JsonProperty]
        public float X {get {return vector.X;} set {vector.X = value;}}
        [JsonProperty]
        public float Y {get {return vector.Y;} set {vector.Y = value;}}
        [JsonProperty]
        public float SpeedX {get {return speed.X;} set {speed.X = value;}}
        [JsonProperty]
        public float SpeedY {get {return speed.Y;} set {speed.Y = value;}}
        [JsonProperty]
        public bool IsAlive {get;set;}
        [JsonProperty]
        public string TextureName{get;set;}

        public bool CheckCollision(Entity other)
        {
            Rectangle myRect = new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(Texture.Width), Convert.ToInt32(Texture.Height));
            Rectangle otherRect = new Rectangle(Convert.ToInt32(other.X), Convert.ToInt32(other.Y), Convert.ToInt32(other.Texture.Width), Convert.ToInt32(other.Texture.Height));
            return myRect.Intersects(otherRect);
        }
    }
}