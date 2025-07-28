using Entitas;
using UnityEngine;

namespace Game.Battle
{
    public class TowerTargetSelectionSystem : IExecuteSystem
    {
        private readonly GameContext _context;
        private readonly IGroup<GameEntity> _towers;
        private readonly IGroup<GameEntity> _enemies;
        private readonly IGroup<GameEntity> _players;

        public TowerTargetSelectionSystem(GameContext context)
        {
            _context = context;

            _towers = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.TowerEnum,
                GameMatcher.WorldPosition,
                GameMatcher.AttackRange
            ));

            _enemies = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Enemy,
                GameMatcher.WorldPosition,
                GameMatcher.CurrentHealthPoints
            ));

            _players = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Player,
                GameMatcher.Id
            ));
        }

        public void Execute()
        {
            foreach (var tower in _towers)
            {
                if (!tower.hasTargetId || !IsValidTarget(tower, tower.TargetId))
                {
                    SelectNewTarget(tower);
                }
            }
        }

        private void SelectNewTarget(GameEntity tower)
        {
            GameEntity closestEnemy = null;
            float closestDistance = float.MaxValue;

            foreach (var player in _players)
            foreach (var enemy in _enemies)
            {
                if (tower.PlayerId != player.Id)
                    continue;

                if (enemy.PlayerId != player.Id)
                    continue;

                if (enemy.CurrentHealthPoints <= 0)
                    continue;

                float distance = Vector3.Distance(tower.WorldPosition, enemy.WorldPosition);

                if (distance <= tower.AttackRange && distance < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }

            if (closestEnemy != null)
                tower.ReplaceTargetId(closestEnemy.Id);
            else if (tower.hasTargetId)
                tower.RemoveTargetId();
        }

        private bool IsValidTarget(GameEntity tower, int targetId)
        {
            var target = _context.GetEntityWithId(targetId);

            if (target == null)
                return false;

            if (target.CurrentHealthPoints <= 0)
                return false;

            float distance = Vector3.Distance(tower.WorldPosition, target.WorldPosition);

            return distance <= tower.AttackRange;
        }
    }
}