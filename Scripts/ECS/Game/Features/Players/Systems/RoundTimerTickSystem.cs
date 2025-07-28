using Entitas;
using Services.Times;

namespace Game
{
    public class RoundTimerTickSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _aliveEnemies;
        private readonly ITimeService _time;

        public RoundTimerTickSystem(GameContext game, ITimeService time)
        {
            _players = game.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.RoundTimer));
            _aliveEnemies = game.GetGroup(GameMatcher.AllOf(GameMatcher.Enemy).NoneOf(GameMatcher.Dead));
            _time = time;
        }

        public void Execute()
        {
            foreach (var player in _players)
            {
                bool hasAlive = false;
                foreach (var enemy in _aliveEnemies)
                {
                    if (enemy.PlayerId == player.Id)
                    {
                        hasAlive = true;
                        break;
                    }
                }

                if (hasAlive)
                    player.ReplaceRoundTimer(player.RoundTimer + _time.DeltaTime);
            }
        }
    }
}