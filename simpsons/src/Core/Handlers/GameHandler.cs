using System.Collections.Generic;
using simpsons.Core;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace simpsons.Core.Handlers
{
    class GameHandler : IDisposable
    {
        private static string SerializeFilePath = "Test.json";
        public string GameID {get;set;}
        public int Score {get;set;}
        public Player Player {get;set;}
        public List<Enemy> Enemies {get;set;}

        public void GenerateGameID()
        {
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
            //Check if file exists, if not then create and serialize object
            if(!File.Exists(SerializeFilePath))
            {
                gameList = new List<GameHandler>();
                gameList.Add(this);
                string serializedJson = JsonConvert.SerializeObject(gameList, Formatting.Indented,
                new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Auto});
                File.WriteAllText(SerializeFilePath, serializedJson);
            }
            else
            {
                bool isFound = false;
                string json = File.ReadAllText(SerializeFilePath);
                
                //Try to create a instance of gamelist, then serialize the data
                gameList = JsonConvert.DeserializeObject<List<GameHandler>>(json,
                    new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto});
                if(gameList == null)
                    gameList = new List<GameHandler>();
                int index = 0;
                foreach((GameHandler gh, Int32 i) in gameList.Where(gh => gh.GameID == this.GameID).Select((gh, i) => (gh, i)))
                {
                    isFound = true;
                    index = i;
                }
                if(isFound)
                {
                    gameList[index] = this;
                }
                else
                {
                    gameList.Add(this);
                }
                
                string jsonOutput = JsonConvert.SerializeObject(gameList, Formatting.Indented,
                new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto});
                File.WriteAllText(SerializeFilePath, jsonOutput);
            }
            
            
        }
        //Denna funktion behöver inte ett objekt för att köras utan är kopplad till typen direkt.
        public static List<GameHandler> DeserializeOnStartup()
        {
            List<GameHandler> gameList;
            if(!File.Exists("Test.json"))
            {
                File.WriteAllText("Test.json","");
            }
            string json = File.ReadAllText("Test.json");
              
            gameList = JsonConvert.DeserializeObject<List<GameHandler>>(json,
            new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto});

            if(gameList == null)
                gameList = new List<GameHandler>();
            
            

            return gameList;
        }

        public void Dispose()
        {
            GameID = null;
            Player = null;
            Enemies = null;
        }
        
    }
}