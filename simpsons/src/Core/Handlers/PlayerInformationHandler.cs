using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;
namespace simpsons.Core.Handlers
{
    //Requires all values to be present in the json
    [JsonObject(ItemRequired = Required.Always)]
    public class PlayerInformationHandler
    {
        const string SerializeFilePath = "Data/Player.json";

        
        public int Cash {get;set;}
        public string SelectedPlayer {get;set;}
        public string SelectedBullet {get;set;}
        public List<string> UnlockedPlayers {get;set;}
        public List<string> UnlockedBullets {get;set;}
        public bool UnlockedCompanion {get;set;}
        public int TotalKills {get;set;}
        public int Deaths {get;set;}
        public int Damage {get;set;}

        private string _defaultPlayer = "Player\\homer";

        public void SerializePlayerData()
        {
            string output = JsonConvert.SerializeObject(this, Formatting.Indented,
            new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto});
            File.WriteAllText(SerializeFilePath, output);
        }
        public void VerifyUnlockedPlayer()
        {
            if(!UnlockedPlayers.Contains(SelectedPlayer))
                SelectedPlayer = _defaultPlayer;
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
                playerInformationHandler = SetDefaultData();
                
            return playerInformationHandler;
        }
        public static PlayerInformationHandler SetDefaultData()
        {
            PlayerInformationHandler playerInformationHandler = new PlayerInformationHandler();
            playerInformationHandler.SelectedPlayer = "Player\\homer";
            playerInformationHandler.SelectedBullet = "Player\\defaultbullet";
            playerInformationHandler.UnlockedBullets = new List<string>(){"Player\\defaultbullet"};
            playerInformationHandler.UnlockedPlayers = new List<string>(){"Player\\homer"};
            playerInformationHandler.SerializePlayerData();
            playerInformationHandler.Damage = 1;
            
            return playerInformationHandler;
        }
    }
}