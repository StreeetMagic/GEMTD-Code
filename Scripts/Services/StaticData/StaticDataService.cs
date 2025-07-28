using System;
using System.Collections.Generic;
using System.Linq;
using Game.Battle;
using Game.Battle.Configs;
using Game.Enemies;
using Game.GameMainFeature;
using Game.Towers;
using Infrastructure;
using Infrastructure.Projects;
using Services.AssetProviders;
using Services.AudioServices.AudioMixers;
using Services.AudioServices.Sounds;
using Services.AudioServices.Sounds.Configs;
using Tools.MazeDesigner;
using UnityEngine;
using UserInterface.MazeSelectorMenu;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<TowerEnum, TowerConfig> _towers = new();
        private Dictionary<int, EnemyConfig> _enemyConfigs = new();
        private Dictionary<GameModeEnum, EnemyCoefficient> _enemyCoefficientConfig = new();
        private Dictionary<GameModeEnum, MazeViewConfig> _mazeViewConfig;
        private Dictionary<object, MazeDataSO> _mazeDataConfigs;
        private Dictionary<AbilityEnum, AbilitySetup> _playerAbilities;
        private Dictionary<AudioMixerGroupEnum, AudioMixerGroupArtSetup> _audioMixerGroups = new();
        private Dictionary<SoundEnum, SoundArtSetup> _sounds = new();
        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public ProjectConfig ProjectConfig { get; private set; }

        public void LoadAll()
        {
            ProjectConfig = _assetProvider.LoadScriptable<ProjectConfig>(nameof(ProjectConfig));

            LoadEnemies();
            LoadTowers();
            LoadAudioSetups();
            
            ProjectConfig.CameraConfig.Initialize();
            LoadPlayerAbilitySetups();
        }

        private void LoadPlayerAbilitySetups()
        {
            _playerAbilities = ProjectConfig.PlayerConfig.PlayerAbilitySetup
                .ToDictionary(x => x.AbilityEnum, x => x);
        }

        private void LoadEnemies()
        {
            _enemyConfigs = Resources
                .LoadAll<EnemyConfig>("Enemies")
                .ToDictionary(x => x.Round, x => x);

            LoadTowers();
            LoadEnemyCoefficient();
            LoadMazeViewConfigs();
            LoadMazeDataConfigs();
        }

        public EnemyConfig GetEnemyConfig(int round)
        {
            return !_enemyConfigs.TryGetValue(round, out EnemyConfig config)
                ? _enemyConfigs.Last().Value
                : config;
        }

        public SoundArtSetup GetSoundConfig(SoundEnum soundEnum)
        {
            if (_sounds.TryGetValue(soundEnum, out var soundArtSetup))
                return soundArtSetup;

            throw new Exception($"Sound config for {soundEnum} was not found");
        }

        public AudioMixerGroupEnum GetAudioMixerGroupEnum(SoundArtSetup soundSetup)
        {
            if (_audioMixerGroups.TryGetValue(soundSetup.AudioMixerGroupEnum, out var audioMixerGroupArtSetup))
                return audioMixerGroupArtSetup.Id;

            throw new Exception($"AudioMixerGroup config for {soundSetup.AudioMixerGroupEnum} was not found");
        }

        public AudioMixerGroupArtSetup GetAudioMixerGroupSetup(AudioMixerGroupEnum groupId)
        {
            if (_audioMixerGroups.TryGetValue(groupId, out var audioMixerGroupArtSetup))
                return audioMixerGroupArtSetup;

            throw new Exception($"AudioMixerGroup config for {groupId} was not found");
        }

        public AudioMixerGroupEnum[] GetAudioMixerGroupEnums()
        {
            return _audioMixerGroups.Keys.ToArray();
        }

        public SoundEnum GetSoundByTowerEnum(TowerEnum towerEnum)
        {
            Dictionary<List<TowerEnum>, SoundEnum> projectConfigTowerSoundConfig = ProjectConfig.TowerSoundConfig;

            return projectConfigTowerSoundConfig.Keys
                .Where(towerEnums => towerEnums
                    .Any(tower => tower == towerEnum))
                .Select(towerEnums => projectConfigTowerSoundConfig[towerEnums])
                .FirstOrDefault();
        }

        public TowerConfig GetTowerConfig(TowerEnum towerEnum)
        {
            if (_towers.TryGetValue(towerEnum, out TowerConfig config))
                return config;

            throw new Exception($"Tower config for {towerEnum} was not found");
        }
        
        public float GetTowerDamage(TowerEnum towerEnum)
        {
            if (_towers.TryGetValue(towerEnum, out TowerConfig config))
            {
                List<EffectSetup> setups = config.BasicAttackSetup.EffectSetups;
                
                foreach (EffectSetup setup in setups)
                {
                    if (setup.EffectEnum == EffectEnum.Damage)
                        return setup.Value;
                }
            }

            throw new Exception($"Tower config or Effect Damage for {towerEnum} was not found`");
        }
        
        public string GetTowerName(TowerEnum towerEnum)
        {
            if (_towers.TryGetValue(towerEnum, out TowerConfig config))
            {
                return config.Name;
            }

            throw new Exception($"Tower config or Effect Damage for {towerEnum} was not found`");
        }
        
        public float GetEnemyDamage(int round)
        {
            return GetEnemyConfig(round).Damage;
        }

        public List<TowerConfig> GetTowerConfigs()
        {
            return _towers.Values.ToList();
        }

        public EnemyCoefficient GetEnemyCoefficient(GameModeEnum gameMode)
        {
            if (_enemyCoefficientConfig.TryGetValue(gameMode, out EnemyCoefficient enemyCoefficient))
                return enemyCoefficient;

            throw new Exception($"EnemyCoefficient for {gameMode} was not found");
        }

        public AbilitySetup GetPlayerAbilitySetup(AbilityEnum playerAbilityEnum)
        {
            if (_playerAbilities.TryGetValue(playerAbilityEnum, out AbilitySetup playerAbilitySetup))
                return playerAbilitySetup;

            throw new Exception($"PlayerAbility setup for {playerAbilityEnum} was not found");
        }

        public MazeViewConfig GetMazeViewConfig(GameModeEnum gameModeEnum)
        {
            if (_mazeViewConfig.TryGetValue(gameModeEnum, out MazeViewConfig mazeViewConfig))
                return mazeViewConfig;

            throw new Exception($"MazeViewConfig for {gameModeEnum} was not found");
        }

        private void LoadMazeViewConfigs()
        {
            _mazeViewConfig = Resources
                .LoadAll<MazeViewConfig>("MazeViewConfigs")
                .ToDictionary(x => x.GameModeEnum, x => x);
        }

        private void LoadMazeDataConfigs()
        {
            // _mazeDataConfigs  = Resources
            //     .LoadAll<MazeDataSO>("MazeViewConfig")
            //     .ToDictionary(x => x., x => x);
        }

        private void LoadAudioSetups()
        {
            LoadAudioMixerGroups();
            LoadSoundSetups();
        }

        private void LoadTowers()
        {
            _towers = Resources
                .LoadAll<TowerConfig>("Towers")
                .ToDictionary(x => x.TowerEnum, x => x);
        }

        private void LoadEnemyCoefficient()
        {
            _enemyCoefficientConfig = Resources
                .LoadAll<EnemyCoefficientConfig>("EnemyCoefficient")
                .ToDictionary(x => x.GameModeEnum, x => x.EnemyCoefficient);
        }

        private void LoadAudioMixerGroups()
        {
            foreach (var audioMixerGroupArtSetup in ProjectConfig.AudioMixerGroupConfig.Setups)
            {
                _audioMixerGroups.Add(audioMixerGroupArtSetup.Id, audioMixerGroupArtSetup);
            }
        }

        private void LoadSoundSetups()
        {
            foreach (var soundArtSetup in ProjectConfig.SoundConfig.Setups)
            {
                _sounds.Add(soundArtSetup.Id, soundArtSetup);
            }
        }
    }
}