using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;
namespace simpsons.Core.Handlers
{
    //Requires all values to be present in the json
    [JsonObject(ItemRequired = Required.Always)]
    class PlayerInformationHandler
    {
        const string SerializeFilePath = "Data/Player.json";

        
        public int Cash {get;set;}
        public string SelectedPlayer {get;set;}
        public string SelectedBullet {get;set;}
        public List<string> UnlockedPlayers {get;set;}
        public List<string> UnlockedBullets {get;set;}


        public void SerializePlayerData()
        {
            string output = JsonConvert.SerializeObject(this, Formatting.Indented,
            new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto});
            File.WriteAllText(SerializeFilePath, output);
        }
        public static PlayerInformationHandler Initialize()
        {
            PlayerInformationHandler playerInformationHandler;
            if(!File.Exists(SerializeFilePath))
                File.WriteAllText(SerializeFilePath, "");
            string json = File.ReadAllText(SerializeFilePath);
            //Try to deserialize, if it fails, set it to null and create a new object.
            try
            {
                playerInformationHandler = JsonConvert.DeserializeObject<PlayerInformationHandler>(
                    json, new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto});
            }
            catch
            {
                playerInformationHandler = null;
            }
            
            if(playerInformationHandler == null)
                playerInformationHandler = SetDefaultData(playerInformationHandler);
                
            return playerInformationHandler;
        }
        public static PlayerInformationHandler SetDefaultData(PlayerInformationHandler playerInformationHandler)
        {
            playerInformationHandler = new PlayerInformationHandler();
            playerInformationHandler.Cash = 0;
            playerInformationHandler.SelectedPlayer = "Player/homer";
            playerInformationHandler.SelectedBullet = "Temp/temp";
            playerInformationHandler.UnlockedBullets = new List<string>();
            playerInformationHandler.UnlockedPlayers = new List<string>(){"Player/homer"};
            playerInformationHandler.SerializePlayerData();
            
            return playerInformationHandler;
        }
    }
}