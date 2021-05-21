using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using simpsons.Core.Handlers;
using Newtonsoft.Json;


namespace simpsons.Core
{
    public abstract class PlayerEntity : Entity
    {   
        [JsonProperty]
        public int Health{get;set;}
        [JsonProperty]
        public int HealthMax{get;set;}
        [JsonProperty]
        public string BulletName {get;set;}
        public Texture2D BulletTexture {get;set;}


        public PlayerEntity(string TextureName, float X, float Y, float SpeedX, float SpeedY, string BulletName, int Health):base(TextureName, X, Y, SpeedX, SpeedY)
        {
            this.Health = Health;
            HealthMax = Health;
            this.BulletName = BulletName;
            BulletTexture = TextureHandler.Sprites[BulletName];
        }
    }

}