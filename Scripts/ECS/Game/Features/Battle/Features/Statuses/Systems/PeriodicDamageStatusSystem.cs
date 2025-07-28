using Entitas;
using Services.Times;

namespace Game.Battle
{
    public class PeriodicDamageStatusSystem : IExecuteSystem
    {
        private readonly ITimeService _timeService;
        private readonly IEffectFactory _effectFactory;
        private readonly IGroup<GameEntity> _statuses;

        public PeriodicDamageStatusSystem(GameContext game, ITimeService timeService, IEffectFactory effectFactory)
        {
            _timeService = timeService;
            _effectFactory = effectFactory;
            _statuses = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Status,
                    GameMatcher.Period,
                    GameMatcher.TimeSinceLastTick,
                    GameMatcher.EffectValue,
                    GameMatcher.ProducerId,
                    GameMatcher.TargetId));
        }

        public void Execute()
        {
            foreach (GameEntity status in _statuses)
            {
                if (status.TimeSinceLastTick >= 0)
                {
                    status.ReplaceTimeSinceLastTick(status.TimeSinceLastTick - _timeService.DeltaTime);
                }
                else
                {
                    status.ReplaceTimeSinceLastTick(status.Period);

                    _effectFactory.CreateEffect(
                        new EffectSetup { EffectEnum = EffectEnum.Damage, Value = status.EffectValue },
                        status.ProducerId,
                        status.TargetId);
                }
            }
        }
    }
}