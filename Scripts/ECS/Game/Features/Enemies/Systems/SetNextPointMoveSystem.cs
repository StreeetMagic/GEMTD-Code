using Entitas;
using Game.Extensions;
using Services.ProjectData;
using Services.StaticData;
using Tools.MazeDesigner;
using UnityEngine;

namespace Game.Enemies
{
    public class SetNextPointMoveSystem : IExecuteSystem
    {
        private readonly GameContext _game;

        private readonly IStaticDataService _config;
        private readonly IProjectDataService _projectDataService;

        private readonly IGroup<GameEntity> _enemies;
        private readonly IGroup<GameEntity> _players;

        public SetNextPointMoveSystem(GameContext game,
            IStaticDataService config,
            IProjectDataService projectDataService)
        {
            _game = game;
            _config = config;
            _projectDataService = projectDataService;

            _enemies = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.WorldPosition,
                    GameMatcher.Enemy,
                    GameMatcher.TargetPlaceIndex,
                    GameMatcher.PathNumber));

            _players = game.GetGroup(GameMatcher.AllOf(GameMatcher.Player,
                GameMatcher.GameLoopStateEnum));
        }

        public void Execute()
        {
            foreach (GameEntity player in _players)
            foreach (GameEntity enemy in _enemies)
            {
                if (enemy.PlayerId != player.Id)
                    continue;

                int waveNumber = player.Round;

                int index = waveNumber - 1;

                RoundPathSetup[] setups = _projectDataService.CurrentMazeData.Rounds;

                if (index >= setups.Length)
                    index = setups.Length - 1;

                RoundPathSetup roundPathSetup = setups[index];

                Vector2Int[] currentPath = roundPathSetup.GetPathByRoundIndex(enemy.PathNumber);

                int currentIndex = enemy.TargetPlaceIndex;

                Vector2Int currentTarget = currentPath[currentIndex];

                currentTarget.x += (int)_projectDataService.StartPositions[player.Index].x;
                currentTarget.y += (int)_projectDataService.StartPositions[player.Index].y;

                if (enemy.WorldPosition.SqrDistance(currentTarget) >= _config.ProjectConfig.DistanceToNextPoint)
                    continue;

                if (currentIndex == currentPath.Length - 1)
                    TryAdvanceBlockIndex(enemy, _config.ProjectConfig.TotalCheckPoints);
                else
                    AdvanceToNextPoint(enemy, currentIndex, currentPath.Length);
            }
        }

        private void AdvanceToNextPoint(GameEntity enemy, int currentIndex, int lengthCurrentPath)
        {
            if (enemy.isFlyable)
            {
                // установить последний TargetPlaceIndex из текущего roundPathSetup.GetPathByRoundIndex
                enemy.ReplaceTargetPlaceIndex(lengthCurrentPath - 1);
            }
            else
            {
                int nextIndex = currentIndex + 1;

                enemy.ReplaceTargetPlaceIndex(nextIndex);
            }
        }

        private void TryAdvanceBlockIndex(GameEntity enemy, int directionsCount)
        {
            if (enemy.PathNumber >= directionsCount - 1)
            {
                if (enemy.CurrentHealthPoints <= 0)
                    return;

                enemy.ReplaceCurrentHealthPoints(0);

                GameEntity player = _game.GetEntityWithId(enemy.PlayerId);
                player.ReplaceCurrentHealthPoints(player.CurrentHealthPoints - 5);

                return;
            }

            enemy.ReplaceTargetPlaceIndex(0);
            enemy.ReplacePathNumber(enemy.PathNumber + 1);
        }
    }
}