using System.Collections.Generic;
using Entitas;

namespace Game.Selection.Systems
{
    public class CancelSelectionByEmptyClickSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _raycasted;
        private readonly IGroup<GameEntity> _leftClicks;
        private readonly IGroup<GameEntity> _secelted;
        private readonly List<GameEntity> _buffer = new List<GameEntity>();

        public CancelSelectionByEmptyClickSystem(GameContext game)
        {
            _raycasted = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CanRaycast,
                GameMatcher.Raycasting
            ));

            _leftClicks = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.LeftMouseButtonClick
            ));

            _secelted = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Selected
            ));
        }

        public void Execute()
        {
            foreach (var click in _leftClicks)
            {
                if (_raycasted.count == 0)
                {
                    foreach (var selected in _secelted.GetEntities(_buffer))
                    {
                        selected.isSelected = false;
                    }
                }
            }
        }
    }
}