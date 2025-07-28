using Entitas;
using Services.StaticData;

namespace Game.GameMainFeature
{
    public class ChangeRoundSystem : IExecuteSystem
    {
        private readonly GameContext _gameContext;
        private readonly GameEntityFactories _factories;
        private readonly IStaticDataService _staticDataService;

        private readonly IGroup<GameEntity> _players;

        public ChangeRoundSystem(
            GameContext gameContext,
            GameEntityFactories factories,
            IStaticDataService staticDataService
        )
        {
            _gameContext = gameContext;
            _factories = factories;
            _staticDataService = staticDataService;

            _players = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Round,
                    GameMatcher.Player,
                    GameMatcher.Level
                ));
        }

        public void Execute()
        {
            foreach (GameEntity player in _players)
            {
                GameEntity mainEntity = _gameContext.gameMainEntity;

                if (player.Round <= mainEntity.Round)
                    continue;

                mainEntity.ReplaceRound(player.Round);

                foreach (GameEntity player2 in _players)
                {
                    if (player2.hasRoundTimer)
                        player2.RemoveRoundTimer();

                    int maxEnemies = mainEntity.Round % 10 == 0
                        ? 1
                        : _staticDataService.ProjectConfig.EnemiesPerRound;

                    _factories.CreateRoundComplete(player2.Id, mainEntity.Round, maxEnemies);
                }
            }
        }
    }
}