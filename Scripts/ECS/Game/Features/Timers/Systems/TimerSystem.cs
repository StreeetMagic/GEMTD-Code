using Entitas;
using Services.Times;

namespace Game.Timers
{
    public class TimerSystem : IExecuteSystem
    {
        private readonly ITimeService _time;
        private readonly IGroup<GameEntity> _timers;

        protected TimerSystem(
            ITimeService time,
            GameContext gameContext
        )
        {
            _time = time;

            _timers = gameContext.GetGroup(GameMatcher.Timer);
        }

        public void Execute()
        {
            foreach (GameEntity entity in _timers)
            {
                if (entity.Timer <= 0)
                    continue;

                float timeLeft = entity.Timer - _time.DeltaTime;
                entity.ReplaceTimer(timeLeft);
            }
        }
    }
}