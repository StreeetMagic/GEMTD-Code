using System.Collections.Generic;
using Entitas;

namespace Game.Selection.Systems
{
    public class UnSelectionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _highlighters;
        private readonly List<GameEntity> _buffer = new();

        public UnSelectionSystem(GameContext game)
        {
            _highlighters = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Selected
                    ));
        }

        public void Execute()
        {
            foreach (GameEntity highlighter in _highlighters.GetEntities(_buffer))
            {
                if (_highlighters.count < 2)
                    return;
                
                if (!highlighter.isRaycasting)
                    highlighter.isSelected = false;
            }
        }
    }
}