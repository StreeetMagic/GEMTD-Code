using System.Collections.Generic;
using Entitas;

namespace Game.PortraitCameras.Systems
{
    public class SetPortraitTargetSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _selectedEntities;
        private readonly IGroup<GameEntity> _playerThrones;
        private readonly List<GameEntity> _buffer = new();
        private readonly IGroup<GameEntity> _portraits;

        public SetPortraitTargetSystem(GameContext context)
        {
            _selectedEntities = context.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Selected,
                        GameMatcher.WorldPosition
                    ));

            _playerThrones = context.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.FinishPoint,
                        GameMatcher.Human,
                        GameMatcher.WorldPosition
                    ));

            _portraits = context.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.PortraitTarget
                    ));
        }

        public void Execute()
        {
            foreach (var selectedUnit in _portraits.GetEntities(_buffer))
            {
                selectedUnit.isPortraitTarget = false;
            }

            if (_selectedEntities.count > 1)
            {
                foreach (var throne in _playerThrones)
                {
                    throne.isPortraitTarget = true;
                }

                return;
            }

            var selected = _selectedEntities.GetSingleEntity();

            var towerOrEnemySelection = _selectedEntities.count == 1 && (selected.isTower || selected.isEnemy);

            if (towerOrEnemySelection)
            {
                selected.isPortraitTarget = true;
            }
            else
            {
                foreach (var throne in _playerThrones)
                {
                    throne.isPortraitTarget = true;
                }
            }
        }
    }
}