using System.Collections.Generic;
using simpsons.Core;
using Newtonsoft.Json;
using System;
using System.IO;

namespace simpsons.Core.Handlers
{
    class GameHandler : IDisposable
    {
        public string GameID {get;set;}
        public int Score {get;set;}
        public Player Player {get;set;}
        public List<Enemy> Enemies {get;set;}

        public void GenerateGameID()
        {
            //Creates a GameID for each session.
            //Not yet implemented to interact with other functions.
            string chars = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            char[] newChars = new char[12];
            Random random = new Random();

            for (int i = 0; i < newChars.Length; i++)
            {
                newChars[i] = chars[random.Next(chars.Length)];
            }
            string finalString = new string(newChars);
            GameID = "G-" + finalString;
        }
        
        public void SerializeGame(Player player, List<Enemy> enemies, int score)
        {
            Player = player;
            Enemies = enemies;
            Score = score;
            List<GameHandler> gameList;
            if(!File.Exists("Test.json"))
            {
                gameList = new List<GameHandler>();
                gameList.Add(this);
                string serializedJson = JsonConvert.SerializeObject(gameList, Formatting.Indented,
                new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Auto});
                File.WriteAllText(@"Test.json", serializedJson);
            }
            else
            {
                
                string json = File.ReadAllText("Test.json");
                gameList = JsonConvert.DeserializeObject<List<GameHandler>>(json,
                new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto});
                
                gameList.Add(this);

                string jsonOutput = JsonConvert.SerializeObject(gameList, Formatting.Indented,
                new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto});
                File.WriteAllText("Test.json", jsonOutput);
            }
            
            
        }
        
        public void Dispose()
        {
            GameID = null;
            Player = null;
            Enemies = null;
        }

        
    }
}