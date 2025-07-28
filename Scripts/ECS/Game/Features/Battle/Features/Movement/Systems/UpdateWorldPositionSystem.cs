using Entitas;
using Services.StaticData;
using Services.Times;
using UnityEngine;

namespace Game.Battle
{
    public class UpdateWorldPositionSystem : IExecuteSystem
    {
        private readonly ITimeService _timeService;
        private readonly IGroup<GameEntity> _group;
        private readonly IStaticDataService _staticDataService;

        public UpdateWorldPositionSystem(
            GameContext game,
            ITimeService timeService,
            IStaticDataService staticDataService
        )
        {
            _timeService = timeService;
            _staticDataService = staticDataService;
            _group = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.WorldPosition,
                    GameMatcher.Direction,
                    GameMatcher.MoveSpeedStat,
                    GameMatcher.MovementAvailable
                )
            );
        }

        public void Execute()
        {
            foreach (GameEntity entity in _group)
            {
                Vector3 position = entity.WorldPosition +
                                   entity.Direction * (entity.MoveSpeedStat * _timeService.DeltaTime *
                                                       _staticDataService.ProjectConfig.MoveStatDotaModifier);
                entity.ReplaceWorldPosition(position);
            }
        }
    }
}