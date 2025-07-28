using System.Collections.Generic;
using Entitas;

namespace Game.Battle
{
    public class CleanEmptyTargetsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _actors;

        private readonly GameContext _game;
        private readonly List<GameEntity> _buffer = new(4);

        public CleanEmptyTargetsSystem(GameContext game)
        {
            _game = game;

            _actors = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.TargetId
                )
                .NoneOf(GameMatcher.Destructed)
            );
        }

        public void Execute()
        {
            foreach (GameEntity actor in _actors.GetEntities(_buffer))
            {
                if (_game.GetEntityWithId(actor.TargetId) == null)
                {
                    actor.isDestructed = true;
                }
            }
        }
    }
}