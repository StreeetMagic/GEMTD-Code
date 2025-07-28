using Entitas;

namespace Game.Highlight
{
    public class HighlightSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _highlighters;

        public HighlightSystem(GameContext game)
        {
            _highlighters = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.CanRaycast,
                        GameMatcher.Raycasting
                    )
            );
        }

        public void Execute()
        {
            foreach (GameEntity highlighter in _highlighters.GetEntities())
            { 
                highlighter.isHighlighted = highlighter.isRaycasting;
            }
        }
    }
}