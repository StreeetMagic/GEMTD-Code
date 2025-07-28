using Entitas;
using UnityEngine;

namespace Game.Battle
{
    public class UpdateArmamentDirectionSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _armaments;
        private readonly IGroup<GameEntity> _targets;

        public UpdateArmamentDirectionSystem(GameContext game)
        {
            _game = game;

            _armaments = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Armament,
                    GameMatcher.Direction,
                    GameMatcher.TargetId
                ));
        }

        public void Execute()
        {
            foreach (GameEntity armament in _armaments)
            {
                GameEntity target = _game.GetEntityWithId(armament.TargetId);

                if (target == null)
                    continue;

                Vector3 direction = (target.TargetPointWorldPosition - armament.WorldPosition).normalized;

                armament.ReplaceDirection(direction);
            }
        }
    }
}