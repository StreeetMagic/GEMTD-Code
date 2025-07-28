using Entitas;

namespace Game.Highlight
{
    public class UnHighlightSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _highlighters;

        public UnHighlightSystem(GameContext game)
        {
            _highlighters = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.CanRaycast
                    )
                    .NoneOf(
                        GameMatcher.Raycasting
                    )
            );
        }

        public void Execute()
        {
            foreach (GameEntity highlighter in _highlighters.GetEntities())
            {
                highlighter.isHighlighted = false; 
            }
        }
    }
}