using Entitas;
using UnityEngine;

namespace Game.Towers
{
    public class RotateTowerSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _towers;

        public RotateTowerSystem(GameContext game)
        {
            _game = game;
            _towers = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.TowerEnum,
                    GameMatcher.WorldPosition,
                    GameMatcher.Rotation
                )
            );
        }

        public void Execute()
        {
            foreach (GameEntity tower in _towers)
            {
                if (!tower.hasTargetId)
                    continue;
                
                GameEntity target = _game.GetEntityWithId(tower.TargetId);
                
                Vector3 direction = target.WorldPosition - tower.WorldPosition;
                direction.y = 0;
                Quaternion rotation = Quaternion.LookRotation(direction);
                tower.ReplaceRotation(rotation);
            }
        }
    }
}