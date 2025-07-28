using Entitas;

namespace Game.Selection.Systems
{
    public class SelectionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _highlighters;
        private readonly IGroup<GameEntity> _clicks;

        public SelectionSystem(GameContext game)
        {
            _highlighters = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Highlighted
                    )
            );

            _clicks = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.LeftMouseButtonClick
                    )
            );
        }

        public void Execute()
        {
            foreach (var click in _clicks)
            foreach (GameEntity highlighter in _highlighters.GetEntities())
            {
                highlighter.isSelected = true; 
            }
        }
    }
}