using Entitas;
using Services.StaticData;
using Services.Times;
using UnityEngine;

namespace Game.Battle
{
    public class UpdateRotationSystem : IExecuteSystem
    {
        private readonly ITimeService _timeService;
        private readonly IGroup<GameEntity> _movingEntities;
        private readonly IStaticDataService _staticDataService;

        public UpdateRotationSystem(
            GameContext gameContext,
            ITimeService timeService,
            IStaticDataService staticDataService
        )
        {
            _timeService = timeService;
            _staticDataService = staticDataService;

            _movingEntities = gameContext.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Direction,
                        GameMatcher.RotationSpeed,
                        GameMatcher.Rotation,
                        GameMatcher.TurnedAlongDirection
                    )
            );
        }

        public void Execute()
        {
            foreach (GameEntity movingEntity in _movingEntities)
            {
                if (movingEntity.Direction.sqrMagnitude <= _staticDataService.ProjectConfig.Epsilon)
                    continue;

                Quaternion targetRotation = Quaternion.LookRotation(movingEntity.Direction, Vector3.up);

                Quaternion movingEntityRotation = Quaternion.Slerp(
                    movingEntity.Rotation,
                    targetRotation,
                    movingEntity.RotationSpeed * _timeService.DeltaTime);

                movingEntity.ReplaceRotation(movingEntityRotation);
            }
        }
    }
}