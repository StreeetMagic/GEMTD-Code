using Entitas;
using Game.Battle;
using Services.Identifiers;
using Services.StaticData;

namespace Game.Enemies
{
    public class EnemyDeathSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _enemies;
        private readonly IIdentifierService _identifiers;
        private readonly IStaticDataService _staticDataService;

        public EnemyDeathSystem(
            GameContext game,
            IStaticDataService staticDataService
        )
        {
            _staticDataService = staticDataService;

            _enemies = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Enemy,
                    GameMatcher.Dead,
                    GameMatcher.ProcessingDeath,
                    GameMatcher.Round,
                    GameMatcher.PlayerId
                )
            );
        }

        public void Execute()
        {
            foreach (GameEntity enemy in _enemies)
            {
                enemy.isMovementAvailable = false;
                enemy.isTurnedAlongDirection = false;

                enemy.RemoveTargetCollectionComponents();

                enemy.ReplaceSelfDestructTimer(_staticDataService.ProjectConfig.EnemyDeathAnimationTime);
            }
        }
    }
}