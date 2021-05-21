using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using simpsons.Core.Handlers;
using Microsoft.Xna.Framework;

namespace simpsons.Core
{
    public class SpawnManager
    {
        public int SpawnCap {get;} = 50;
        public Random RandomGenerator {get;set;}
        
        public Random SpawnNumber {get;set;}

        private int defaultValue = 50000;
        private int spawnRate = 500;
        private int finalSpawnValue;

        public SpawnManager()
        {
            RandomGenerator = new Random();
            SpawnNumber = new Random();
        }

        public void Update(List<Enemy> enemies, GameTime gameTime, GameHandler gameHandler)
        {
            finalSpawnValue = defaultValue / (spawnRate / 5) + (int)(gameHandler.TimeInGame / spawnRate); 
            double spawnValue = RandomGenerator.NextDouble();

            if(SpawnNumber.Next(0, finalSpawnValue) == 0)
            {
                if(enemies.Count < SpawnCap)
                {
                    if(spawnValue <= 0.5)
                        enemies.Add(new Bart("Enemies\\bart",10, 10, 0.1f,0.1f,1));
                    if(spawnValue > 0.5 && spawnValue <= 0.8)
                        Console.WriteLine("Less than 0.8 and greater than 0.5");
                    if(spawnValue > 0.8)
                        Console.WriteLine("Greater than 0.8");
                }
            }
            
            
        }
    }
}