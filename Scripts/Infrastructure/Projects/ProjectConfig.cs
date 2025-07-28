using System.Collections.Generic;
using Game;
using Game.Cameras;
using Game.Enemies;
using Game.PlayerAbility.Configs;
using Game.Towers;
using Services.AudioServices.AudioMixers;
using Services.AudioServices.Sounds;
using Services.AudioServices.Sounds.Configs;
using Tools.MazeDesigner;
using UnityEngine;

namespace Infrastructure.Projects
{
    [CreateAssetMenu(fileName = nameof(ProjectConfig), menuName = "Configs/" + nameof(ProjectConfig))]
    public class ProjectConfig : ScriptableObject
    {
        public PlayerConfig PlayerConfig;
        public float MoveStatDotaModifier = 0.01f;
        public float AttackSpeedStatDotaModifier = 1f;
        public float MaxThroneHealthPoint = 100f;
        public float EnemyDeathAnimationTime = 3f;

        [HideInInspector] public MazeDataSO CurrentMaze;

        public int MaxPlayerHealth;

        public int TowersPerRound = 5;
        public EnemyConfig EnemyConfig;
        public CameraConfig CameraConfig;
        public Sprite WallSprite;

        public float SpiritPlacementTime = 0.1f;
        public float PlayerAbilityTime = 0.1f;
        public float ChooseSpiritTime = 0.2f;
        public int MaxPlayerLevel = 5;

        public int TotalCheckPoints = 6;
        public int EnemiesPerRound = 10;
        public float EnemySpawnCooldown = 1;

        public float DistanceToNextPoint = .2f;

        public float Epsilon = 0.001f;

        public float SpiritToTowerTime = 0.2f;

        public float[] TimeMultipliers = { 1f, 2f, 3, 5f };

        public readonly Dictionary<int, List<int>> Chances = new()
        {
            {
                1, new List<int>
                    { 100, 0, 0, 0, 0 }
            },
            {
                2, new List<int>
                    { 80, 20, 0, 0, 0 }
            },
            {
                3, new List<int>
                    { 60, 30, 10, 0, 0 }
            },
            {
                4, new List<int>
                    { 40, 30, 20, 10, 0 }
            },
            {
                5, new List<int>
                    { 10, 30, 20, 20, 10 }
            }
        };

        public readonly Dictionary<TowerEnum, int> TowerLevels = new()
        {
            { TowerEnum.B1, 1 },
            { TowerEnum.B2, 2 },
            { TowerEnum.B3, 3 },
            { TowerEnum.B4, 4 },
            { TowerEnum.B5, 5 },
            { TowerEnum.B6, 6 },

            { TowerEnum.D1, 1 },
            { TowerEnum.D2, 2 },
            { TowerEnum.D3, 3 },
            { TowerEnum.D4, 4 },
            { TowerEnum.D5, 5 },
            { TowerEnum.D6, 6 },

            { TowerEnum.Y1, 1 },
            { TowerEnum.Y2, 2 },
            { TowerEnum.Y3, 3 },
            { TowerEnum.Y4, 4 },
            { TowerEnum.Y5, 5 },
            { TowerEnum.Y6, 6 },

            { TowerEnum.G1, 1 },
            { TowerEnum.G2, 2 },
            { TowerEnum.G3, 3 },
            { TowerEnum.G4, 4 },
            { TowerEnum.G5, 5 },
            { TowerEnum.G6, 6 },

            { TowerEnum.E1, 1 },
            { TowerEnum.E2, 2 },
            { TowerEnum.E3, 3 },
            { TowerEnum.E4, 4 },
            { TowerEnum.E5, 5 },
            { TowerEnum.E6, 6 },

            { TowerEnum.Q1, 1 },
            { TowerEnum.Q2, 2 },
            { TowerEnum.Q3, 3 },
            { TowerEnum.Q4, 4 },
            { TowerEnum.Q5, 5 },
            { TowerEnum.Q6, 6 },

            { TowerEnum.R1, 1 },
            { TowerEnum.R2, 2 },
            { TowerEnum.R3, 3 },
            { TowerEnum.R4, 4 },
            { TowerEnum.R5, 5 },
            { TowerEnum.R6, 6 },

            { TowerEnum.P1, 1 },
            { TowerEnum.P2, 2 },
            { TowerEnum.P3, 3 },
            { TowerEnum.P4, 4 },
            { TowerEnum.P5, 5 },
            { TowerEnum.P6, 6 }
        };

        public readonly Dictionary<int, List<TowerEnum>> Towers = new()
        {
            {
                1, new List<TowerEnum>
                {
                    TowerEnum.B1,
                    TowerEnum.D1,
                    TowerEnum.Y1,
                    TowerEnum.G1,
                    TowerEnum.E1,
                    TowerEnum.Q1,
                    TowerEnum.R1,
                    TowerEnum.P1
                }
            },
            {
                2, new List<TowerEnum>
                {
                    TowerEnum.B2,
                    TowerEnum.D2,
                    TowerEnum.Y2,
                    TowerEnum.G2,
                    TowerEnum.E2,
                    TowerEnum.Q2,
                    TowerEnum.R2,
                    TowerEnum.P2
                }
            },
            {
                3, new List<TowerEnum>
                {
                    TowerEnum.B3,
                    TowerEnum.D3,
                    TowerEnum.Y3,
                    TowerEnum.G3,
                    TowerEnum.E3,
                    TowerEnum.Q3,
                    TowerEnum.R3,
                    TowerEnum.P3
                }
            },
            {
                4, new List<TowerEnum>
                {
                    TowerEnum.B4,
                    TowerEnum.D4,
                    TowerEnum.Y4,
                    TowerEnum.G4,
                    TowerEnum.E4,
                    TowerEnum.Q4,
                    TowerEnum.R4,
                    TowerEnum.P4
                }
            },
            {
                5, new List<TowerEnum>
                {
                    TowerEnum.B5,
                    TowerEnum.D5,
                    TowerEnum.Y5,
                    TowerEnum.G5,
                    TowerEnum.E5,
                    TowerEnum.Q5,
                    TowerEnum.R5,
                    TowerEnum.P5
                }
            },
        };

        public GameObject AudioSourcePrefab;
        public SoundArtConfig SoundConfig;
        public AudioMixerGroupArtConfig AudioMixerGroupConfig;

        public readonly Dictionary<List<TowerEnum>, SoundEnum> TowerSoundConfig = new()
        {
            {
                new List<TowerEnum>
                {
                    TowerEnum.B1,
                    TowerEnum.B2,
                    TowerEnum.B3,
                    TowerEnum.B4,
                    TowerEnum.B5,
                    TowerEnum.B6
                },
                SoundEnum.BMuzzleFlash
            },
            {
                new List<TowerEnum>
                {
                    TowerEnum.D1,
                    TowerEnum.D2,
                    TowerEnum.D3,
                    TowerEnum.D4,
                    TowerEnum.D5,
                    TowerEnum.D6
                },
                SoundEnum.DMuzzleFlash
            },
            {
                new List<TowerEnum>
                {
                    TowerEnum.E1,
                    TowerEnum.E2,
                    TowerEnum.E3,
                    TowerEnum.E4,
                    TowerEnum.E5,
                    TowerEnum.E6
                },
                SoundEnum.EMuzzleFlash
            },
            {
                new List<TowerEnum>
                {
                    TowerEnum.G1,
                    TowerEnum.G2,
                    TowerEnum.G3,
                    TowerEnum.G4,
                    TowerEnum.G5,
                    TowerEnum.G6
                },
                SoundEnum.GMuzzleFlash
            },
            {
                new List<TowerEnum>
                {
                    TowerEnum.P1,
                    TowerEnum.P2,
                    TowerEnum.P3,
                    TowerEnum.P4,
                    TowerEnum.P5,
                    TowerEnum.P6
                },
                SoundEnum.PMuzzleFlash
            },
            {
                new List<TowerEnum>
                {
                    TowerEnum.Q1,
                    TowerEnum.Q2,
                    TowerEnum.Q3,
                    TowerEnum.Q4,
                    TowerEnum.Q5,
                    TowerEnum.Q6
                },
                SoundEnum.QMuzzleFlash
            },
            {
                new List<TowerEnum>
                {
                    TowerEnum.R1,
                    TowerEnum.R2,
                    TowerEnum.R3,
                    TowerEnum.R4,
                    TowerEnum.R5,
                    TowerEnum.R6
                },
                SoundEnum.RMuzzleFlash
            },
            {
                new List<TowerEnum>
                {
                    TowerEnum.Y1,
                    TowerEnum.Y2,
                    TowerEnum.Y3,
                    TowerEnum.Y4,
                    TowerEnum.Y5,
                    TowerEnum.Y6
                },
                SoundEnum.YMuzzleFlash
            },
        };

        public GameEntityView CoinDropsEffect;
    }
}