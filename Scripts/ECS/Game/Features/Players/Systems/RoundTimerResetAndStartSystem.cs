using Entitas;

namespace Game
{
    public class RoundTimerResetAndStartSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _enemies;

        public RoundTimerResetAndStartSystem(GameContext game)
        {
            _players = game.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.RoundTimer));
            _enemies = game.GetGroup(GameMatcher.AllOf(GameMatcher.Enemy, GameMatcher.Age));
        }

        public void Execute()
        {
            foreach (var player in _players)
            {
                if (player.RoundTimer > 0f)
                    continue;

                foreach (var enemy in _enemies)
                {
                    if (enemy.PlayerId != player.Id)
                        continue;

                    player.ReplaceRoundTimer(0.01f);
                    break;
                }
            }
        }
    }
}