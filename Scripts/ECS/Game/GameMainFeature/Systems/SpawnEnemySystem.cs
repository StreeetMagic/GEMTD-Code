using Entitas;
using Services.ProjectData;
using Services.StaticData;
using UnityEngine;

namespace Game.GameMainFeature
{
    public class SpawnEnemySystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IStaticDataService _staticDataService;
        private readonly GameEntityFactories _factories;
        private readonly IGroup<GameEntity> _players;
        private readonly IProjectDataService _projectDataService;

        public SpawnEnemySystem(
            GameContext game,
            IStaticDataService staticDataService,
            GameEntityFactories factories, 
            IProjectDataService projectDataService
            )
        {
            _game = game;
            _staticDataService = staticDataService;
            _factories = factories;
            _projectDataService = projectDataService;

            _players = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.GameLoopStateEnum
                ));
        }

        public void Execute()
        {
            GameEntity spawner = _game.gameMainEntity;

            int baseEnemies = _staticDataService.ProjectConfig.EnemiesPerRound;
            int bossRoundsPassed = spawner.Round / 10;
            int spawnLimit = baseEnemies * spawner.Round - (baseEnemies - 1) * bossRoundsPassed;

            if (spawner.EnemySpawned >= spawnLimit)
                return;
            
            bool anyPlayerInBattleState = false;
            
            foreach (GameEntity player in _players)
            {
                if (player.gameLoopStateEnum.Value == GameLoopStateEnum.KillEnemy)
                    anyPlayerInBattleState = true;
            }
            
            if (anyPlayerInBattleState == false)
                return;

            foreach (GameEntity player in _players)
            {
                if (spawner.hasTimer == false)
                    spawner.AddTimer(_staticDataService.ProjectConfig.EnemySpawnCooldown);

                if (spawner.Timer > 0f)
                    return;

                Spawn(spawner, player);
            }

            spawner.ReplaceTimer(_staticDataService.ProjectConfig.EnemySpawnCooldown);
            spawner.ReplaceEnemySpawned(spawner.EnemySpawned + 1);
        }

        private void Spawn(GameEntity spawner, GameEntity player)
        {
            Vector2Int position = _projectDataService.CurrentMazeData.Rounds[0].StartToCheckPoint1[0];

            position.x += (int)_projectDataService.StartPositions[player.Index].x;
            position.y += (int)_projectDataService.StartPositions[player.Index].y;

            _factories.CreateEnemy(position, spawner.Round, player.Id, player.isHuman);
        }
    }
}
