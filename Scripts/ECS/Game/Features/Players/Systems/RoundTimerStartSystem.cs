using Entitas;

namespace Game
{
    public class RoundTimerStartSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _enemies;

        public RoundTimerStartSystem(GameContext game)
        {
            _players = game.GetGroup(GameMatcher.Player);
            _enemies = game.GetGroup(GameMatcher.AllOf(GameMatcher.Enemy, GameMatcher.Age).NoneOf(GameMatcher.Dead));
        }

        public void Execute()
        {
            foreach (var player in _players)
            {
                if (player.hasRoundTimer) continue;

                foreach (var enemy in _enemies)
                {
                    if (enemy.PlayerId != player.Id)
                        continue;

                    if (enemy.Age < 0.05f)
                    {
                        player.AddRoundTimer(0f);
                        break;
                    }
                }
            }
        }
    }
}