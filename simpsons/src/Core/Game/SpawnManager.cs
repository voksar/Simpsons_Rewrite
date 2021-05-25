using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using simpsons.Core.Handlers;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using simpsons.Core.Utils;

namespace simpsons.Core
{
    public class SpawnManager
    {


        public int SpawnCap {get;} = 50;
        public Random RandomGenerator {get;set;}
        
        public Random SpawnNumber {get;set;}

        private int defaultValue = 250000;
        private int spawnRate = 100;
        private int finalSpawnValue;

        public SpawnManager()
        {
            RandomGenerator = new Random();
            SpawnNumber = new Random();
        }

        public void Update(ObservableCollection<Enemy> enemies, GameTime gameTime, GameHandler gameHandler)
        {
            finalSpawnValue = defaultValue / (spawnRate + (int)(gameHandler.TimeInGame / spawnRate * 5)); 
            double spawnValue = RandomGenerator.NextDouble();
            
            if(SpawnNumber.Next(0, finalSpawnValue) == 0)
            {
                if(enemies.Count < SpawnCap)
                {   
                    int spawnX, spawnXMax, spawnPoint;

                    spawnX = 100;
                    spawnXMax = ResolutionUtils.Width - 100;
                    spawnPoint = RandomGenerator.Next(spawnX, spawnXMax);

                    if(spawnValue <= 0.5)
                    {
                        
                        enemies.Add(new Bart("Enemies/bart", spawnPoint, -50, 1));
                    }
                        
                    if(spawnValue > 0.5 && spawnValue <= 0.8)
                        return;
                    if(spawnValue > 0.8)
                        return;
                }
            }
            
            
        }
    }
}