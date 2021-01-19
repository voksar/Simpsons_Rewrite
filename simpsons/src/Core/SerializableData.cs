using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace simpsons
{
    static class SerializeGame
    {
        public static void SerializeCurrentGame(Player player, List<Enemy> enemies)
        {
            GameInformationHandler gameInformationHandler = new GameInformationHandler();
            gameInformationHandler.player = player;
            gameInformationHandler.enemies = enemies;

            string output = JsonConvert.SerializeObject(gameInformationHandler, Formatting.Indented,
            new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Auto});

            File.WriteAllText(@"Test.json", output);
        }
    }
    [JsonObject(MemberSerialization.OptOut)]
    class GameInformationHandler
    {
        public Player player{get;set;}
        public List<Enemy> enemies{get;set;}
    }
}