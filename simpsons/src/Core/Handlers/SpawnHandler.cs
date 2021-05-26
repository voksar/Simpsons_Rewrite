using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using simpsons.Core.Handlers;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using simpsons.Core.Utils;
using simpsons.Core.Entities;

namespace simpsons.Core.Handlers
{
    public class SpawnHandler
    {


        public int SpawnCap {get;} = 50;
        public bool SpawnAllowed {get;set;} = true;

        private Random _randomGenerator {get;set;}
        private Random _spawnNumber {get;set;}
        private int _defaultValue = 250000;
        private int _spawnRate = 100;
        private int _finalSpawnValue;

        
        private GameHandler _gameHandler;

        

        public SpawnHandler(GameHandler gameHandler)
        {
            _randomGenerator = new Random();
            _spawnNumber = new Random();
            _gameHandler = gameHandler;
        }

        public void Update(ObservableCollection<Enemy> enemies, GameTime gameTime)
        {
            _finalSpawnValue = _defaultValue / (_spawnRate + (int)(_gameHandler.TimeInGame / _spawnRate * 5)); 
            double spawnValue = _randomGenerator.NextDouble();

            if(_spawnNumber.Next(0, _finalSpawnValue) == 0)
            {
                if(enemies.Count <= SpawnCap && SpawnAllowed)
                {   
                    int spawnX, spawnXMax, spawnPoint;

                    spawnX = 100;
                    spawnXMax = ResolutionUtils.Width - 100;
                    spawnPoint = _randomGenerator.Next(spawnX, spawnXMax);

                    if(spawnValue <= 0.5)
                    {
                        
                        enemies.Add(new Bart("Enemies/bart", spawnPoint, -50, 1));
                    }
                        
                    if(spawnValue > 0.5 && spawnValue <= 0.8)
                        return;
                    if(spawnValue > 0.8)
                        return;
                }
                else if(!SpawnAllowed)
                {
                    throw new NotImplementedException();
                }
            }
            
            
        }
    }
}