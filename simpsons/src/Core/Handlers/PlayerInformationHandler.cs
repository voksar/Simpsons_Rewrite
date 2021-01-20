using System.Collections.Generic;
using Newtonsoft.Json;

namespace simpsons.Core.Handlers
{
    class PlayerInformationHandler
    {
        const string SerializeFilePath = "PlayerData.json";
        public int Cash {get;set;}
        public string SelectedPlayer {get;set;}
        public string SelectedBullet {get;set;}
        public List<object> UnlockedPlayers {get;set;}
        public List<object> UnlockedBullets {get;set;}

        public static PlayerInformationHandler Initialize()
        {
            PlayerInformationHandler playerInformationHandler = JsonConvert.DeserializeObject<PlayerInformationHandler>(
                SerializeFilePath, new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Auto}
            );

            return playerInformationHandler;
        }
    }
}