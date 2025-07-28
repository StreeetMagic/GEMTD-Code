using System.Collections.Generic;
using Entitas;

namespace Game.Towers.SelectSpirits.Systems
{
    public class SelectSpiritSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly SpiritFactory _spiritFactory;
        private readonly IGroup<GameEntity> _spiritRequest;

        private readonly List<GameEntity> _buffer = new(4);

        public SelectSpiritSystem(GameContext game, SpiritFactory spiritFactory)
        {
            _game = game;
            _spiritFactory = spiritFactory;

            _spiritRequest = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.RequestSpiritSelect,
                    GameMatcher.PlayerId,
                    GameMatcher.WorldPosition,
                    GameMatcher.MazePosition,
                    GameMatcher.TowerEnum
                ));
        }

        public void Execute()
        {
            foreach (GameEntity spiritRequest in _spiritRequest.GetEntities(_buffer))
            {
                GameEntity selectedSpirit = _spiritFactory.CreateSpirit(
                    (int)spiritRequest.WorldPosition.x,
                    (int)spiritRequest.WorldPosition.z,
                    spiritRequest.MazePosition.x,
                    spiritRequest.MazePosition.y,
                    spiritRequest.TowerEnum,
                    gameLoopWave: _game.gameMainEntity.Round,
                    spiritRequest.PlayerId
                );

                selectedSpirit.isChosen = true;
                spiritRequest.isDestructed = true;
            }
        }
    }
}