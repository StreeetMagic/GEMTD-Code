using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Configs;
using Game.Enemies;
using Game.GameMainFeature;
using Game.Towers;
using Infrastructure.Projects;
using Services.AudioServices.AudioMixers;
using Services.AudioServices.Sounds;
using Services.AudioServices.Sounds.Configs;
using UserInterface.MazeSelectorMenu;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        ProjectConfig ProjectConfig { get; }

        void LoadAll();

        TowerConfig GetTowerConfig(TowerEnum towerEnum);

        EnemyConfig GetEnemyConfig(int round);

        EnemyCoefficient GetEnemyCoefficient(GameModeEnum gameMode);

        MazeViewConfig GetMazeViewConfig(GameModeEnum gameModeEnum);

        List<TowerConfig> GetTowerConfigs();
        AbilitySetup GetPlayerAbilitySetup(AbilityEnum playerAbilityEnum);
        float GetTowerDamage(TowerEnum towerEnum);
        float GetEnemyDamage(int round);
        string GetTowerName(TowerEnum towerEnum);

        SoundArtSetup GetSoundConfig(SoundEnum soundEnum);

        AudioMixerGroupEnum GetAudioMixerGroupEnum(SoundArtSetup soundSetup);

        AudioMixerGroupArtSetup GetAudioMixerGroupSetup(AudioMixerGroupEnum groupId);

        AudioMixerGroupEnum[] GetAudioMixerGroupEnums();

        SoundEnum GetSoundByTowerEnum(TowerEnum towerEnum);
    }
}