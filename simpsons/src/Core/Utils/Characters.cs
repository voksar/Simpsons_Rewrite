using System.Collections.Generic;

namespace simpsons.Core.Utils
{
    public static class CharacterList
    {
        

        public static Dictionary<string, int> Characters {get; private set;} = new Dictionary<string, int>()
        {
            { "Player\\homer", 0 },
            { "Player\\lisa", 2500 },
            { "Player\\marge", 5000 }
        };
    }
}