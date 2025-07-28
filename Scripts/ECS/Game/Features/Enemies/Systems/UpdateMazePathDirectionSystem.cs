using Entitas;
using Services.ProjectData;
using Services.StaticData;
using Tools.MazeDesigner;
using UnityEngine;

namespace Game.Enemies
{
    public class UpdateMazePathDirectionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _enemies;
        private readonly IGroup<GameEntity> _players;
        private readonly IStaticDataService _config;
        private readonly IProjectDataService _projectDataService;

        public UpdateMazePathDirectionSystem(
            GameContext game,
            IStaticDataService config,
            IProjectDataService projectDataService
        )
        {
            _config = config;
            _projectDataService = projectDataService;

            _enemies = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Direction,
                GameMatcher.TargetPlaceIndex,
                GameMatcher.PathNumber,
                GameMatcher.PlayerId,
                GameMatcher.Enemy
            ));

            _players = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.GameLoopStateEnum
                ));
        }

        public void Execute()
        {
            foreach (GameEntity player in _players)
            foreach (GameEntity enemy in _enemies)
            {
                if (enemy.PlayerId != player.Id)
                    continue;

                int wave = player.Round;

                int index = wave - 1;

                if (index >= _projectDataService.CurrentMazeData.Rounds.Length)
                    index = _projectDataService.CurrentMazeData.Rounds.Length - 1;

                RoundPathSetup path = _projectDataService.CurrentMazeData.Rounds[index];

                Vector2Int targetVector2 = path.GetPosition(enemy.PathNumber, enemy.TargetPlaceIndex);

                targetVector2.x += (int)_projectDataService.StartPositions[player.Index].x;
                targetVector2.y += (int)_projectDataService.StartPositions[player.Index].y;

                Vector3 targetPosition = new Vector3(targetVector2.x, 0, targetVector2.y);

                enemy.ReplaceDirection((targetPosition - enemy.WorldPosition).normalized);
            }
        }
    }
}