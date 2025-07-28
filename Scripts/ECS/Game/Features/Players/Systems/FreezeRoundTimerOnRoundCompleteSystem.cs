using Entitas;

namespace Game
{
    public class FreezeRoundTimerOnRoundCompleteSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _roundCompletes;
        private readonly IGroup<GameEntity> _players;

        public FreezeRoundTimerOnRoundCompleteSystem(GameContext game)
        {
            _roundCompletes = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.RoundComplete,
                    GameMatcher.EnemiesKilled,
                    GameMatcher.EnemiesPerRound
                ));

            _players = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Player,
                    GameMatcher.RoundTimer
                ));
        }

        public void Execute()
        {
            foreach (var roundComplete in _roundCompletes)
            foreach (var player in _players)
            {
                if (player.Id != roundComplete.PlayerId)
                    continue;

                if (roundComplete.EnemiesKilled >= roundComplete.EnemiesPerRound)
                    return;
            }
        }
    }
}