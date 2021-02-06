using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using simpsons.Core.Handlers;

namespace simpsons.Core
{
    public class SpawnManager
    {
        public int SpawnCap {get;} = 50;
        public Random RandomGenerator {get;set;}
        

        public SpawnManager()
        {
            RandomGenerator = new Random();
        }

        public void Update(List<Enemy> enemies)
        {
            double spawnValue = RandomGenerator.NextDouble();

            if(enemies.Count < SpawnCap)
            {
                if(spawnValue <= 0.5)
                    enemies.Add(new Bart("Enemies/bart",10, 10, 0.1f,0.1f,1));
                if(spawnValue > 0.5 && spawnValue <= 0.8)
                    Console.WriteLine("Less than 0.8 and greater than 0.5");
                if(spawnValue > 0.8)
                    Console.WriteLine("Greater than 0.8");
            }
            
        }
    }
}