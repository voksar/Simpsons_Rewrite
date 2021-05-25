using System.Collections.Generic;

namespace simpsons.Core.Utils
{
    public static class CharacterList
    {
        

        public static Dictionary<string, int> Characters {get; private set;} = new Dictionary<string, int>()
        {
            { "Player/homer", 0 },
            { "Player/lisa", 2500 },
            { "Player/marge", 5000 },
            { "Player/krusty", 10000 },
            { "Player/bob", 25000 },
            { "Player/flanders", 50000 }
        };
    }
}