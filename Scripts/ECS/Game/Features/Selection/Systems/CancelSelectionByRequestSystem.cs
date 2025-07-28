using System.Collections.Generic;
using Entitas;

namespace Game.Selection.Systems
{
    public class CancelSelectionByRequestSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _requests;
        private readonly IGroup<GameEntity> _selected;
        private readonly List<GameEntity> _buffer = new();

        public CancelSelectionByRequestSystem(GameContext game)
        {
            _requests = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.CancelSelectionRequest
                    )
            );
            
            _selected = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Selected
                    )
            );
        }

        public void Execute()
        {
            foreach (GameEntity request in _requests)
            foreach (GameEntity highlighter in _selected.GetEntities(_buffer))
            {
                highlighter.isSelected = false;
            }
        }
    }
}