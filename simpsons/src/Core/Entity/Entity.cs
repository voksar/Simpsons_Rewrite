using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Newtonsoft.Json;
using simpsons.Core.Handlers;

namespace simpsons.Core.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Entity
    {
        protected Vector2 vector;
        protected Vector2 speed;
        public Texture2D Texture {get;set;}
        //Serializable data below
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
        [JsonConstructor]
        public Entity(string TextureName, float X, float Y, float SpeedX, float SpeedY)
        {
            Texture = TextureHandler.Sprites[TextureName];
            this.speed.X = SpeedX;
            this.speed.Y = SpeedY;
            vector.X = X;
            vector.Y = Y;
            IsAlive = true;
            this.TextureName = TextureName;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, vector, Color.White);
        }
        public bool CheckCollision(Entity other)
        {
            Rectangle myRect = new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(Texture.Width), Convert.ToInt32(Texture.Height));
            Rectangle otherRect = new Rectangle(Convert.ToInt32(other.X), Convert.ToInt32(other.Y), Convert.ToInt32(other.Texture.Width), Convert.ToInt32(other.Texture.Height));
            return myRect.Intersects(otherRect);
        }
    }
}