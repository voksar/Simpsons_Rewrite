using System.Collections.Generic;
using simpsons.Core;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace simpsons.Core.Handlers
{
    [JsonObject(ItemRequired = Required.Always)]
    class GameHandler : IDisposable
    {
        private static string SerializeFilePath = "Test.json";
        public string GameID {get;set;}
        public double TimeInGame {get;set;}
        public int Score {get;set;}
        public Player Player {get;set;}
        public List<Enemy> Enemies {get;set;}


        public void SetProperties(Player player, List<Enemy> enemies, int score)
        {
            Player = player;
            Enemies = enemies;
            Score = score;
        }
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
        
        public static void SerializeGame(List<GameHandler> gameHandlers)
        {
            //Check if file exists, if not then create and serialize object
            if(!File.Exists(SerializeFilePath))
                File.WriteAllText(SerializeFilePath, "");

            string output = JsonConvert.SerializeObject(gameHandlers, 
            Formatting.Indented, new JsonSerializerSettings(){
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText(SerializeFilePath, output);
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

        public static List<GameHandler> AddDataToTable(GameHandler gameHandler, List<GameHandler> gameHandlers, DisplayGames displayGames)
        {
            
            int index = gameHandlers.FindIndex(item => item.GameID == gameHandler.GameID);
            if(index == -1)
            {
                displayGames.AddGameItem(gameHandler);
                gameHandlers.Add(gameHandler);
            }
            else
            {
                gameHandlers[index] = gameHandler;
                displayGames.displayGamesItems[index].Game = gameHandler;
            }
            
            return gameHandlers;
        }

        public void Dispose()
        {
            GameID = null;
            Player = null;
            Enemies = null;
        }
        
    }
}