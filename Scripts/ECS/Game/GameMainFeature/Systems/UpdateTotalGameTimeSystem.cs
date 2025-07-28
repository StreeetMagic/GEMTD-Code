using Entitas;
using Services.Times;

namespace Game.GameMainFeature
{
    public class UpdateTotalGameTimeSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly ITimeService _timeService;

        public UpdateTotalGameTimeSystem(GameContext context, ITimeService timeService)
        {
            _players = context.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Player,
                    GameMatcher.Human,
                    GameMatcher.TotalGameTime
                ));
            _timeService = timeService;
        }

        public void Execute()
        {
            float dt = _timeService.DeltaTime;

            foreach (var player in _players)
            {
                player.ReplaceTotalGameTime(player.TotalGameTime + dt);
            }
        }
    }
}