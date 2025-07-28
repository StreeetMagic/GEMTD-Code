using System.Collections.Generic;
using Entitas;

namespace Game.Towers.MergeSpirits.Systems
{
    public class FinilizeMergeSpiritsSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly SpiritFactory _spiritFactory;
        private readonly IGroup<GameEntity> _mergeResults;
        private readonly List<GameEntity> _buffer = new(32);

        public FinilizeMergeSpiritsSystem(GameContext game, SpiritFactory spiritFactory)
        {
            _game = game;
            _spiritFactory = spiritFactory;
            
            _mergeResults = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.WorldPosition,
                    GameMatcher.TowerEnum,
                    GameMatcher.MazePosition,
                    GameMatcher.MergeResult
                ));
        }
        
        public void Execute()
        {
            foreach (GameEntity result in _mergeResults.GetEntities(_buffer))
            {
                GameEntity mergedSpirit = _spiritFactory.CreateSpirit(
                    (int)result.WorldPosition.x,
                    (int)result.WorldPosition.z,
                    result.MazePosition.x,
                    result.MazePosition.y,
                    result.TowerEnum,
                    gameLoopWave: _game.gameMainEntity.Round,
                    result.PlayerId
                );

                mergedSpirit.isChosen = true;
                result.isDestructed = true;
            }
        }
    }
}