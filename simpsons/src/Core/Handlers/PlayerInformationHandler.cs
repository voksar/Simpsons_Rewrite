using System.Collections.Generic;

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
    }
}