using System;
using System.Collections.Generic;
using Game.Towers;
using NUnit.Framework;
using Services.Randoms;
using Services.StaticData;

namespace Services.TowerRandomers
{
    public class TowerRandomer
    {
        private readonly IRandomService _randomService;
        private readonly IStaticDataService _config;

        public TowerRandomer(
            IRandomService randomService,
            IStaticDataService config
        )
        {
            _randomService = randomService;
            _config = config;
        }

        public TowerEnum GetTowerEnum(int playerLevel)
        {
            if (playerLevel < 1)
                throw new Exception("Invalid player level");

            if (playerLevel > _config.ProjectConfig.MaxPlayerLevel)
                throw new Exception("Invalid player level");

            List<int> distribution = _config.ProjectConfig.Chances[playerLevel];

            int rand = _randomService.Range(0, 100);

            int sum = 0;
            int towerLevelIndex = 0;

            for (int i = 0; i < distribution.Count; i++)
            {
                sum += distribution[i];

                if (rand >= sum)
                    continue;

                towerLevelIndex = i;
                break;
            }

            int towerLevel = towerLevelIndex + 1;

            List<TowerEnum> availableTowers = _config.ProjectConfig.Towers[towerLevel];
            int randomTowerIndex = _randomService.Range(0, availableTowers.Count);

            return availableTowers[randomTowerIndex];
        }
    }
}