using Entitas;

namespace Game.Battle
{
    public class DestroyAbilityEntitiesOnUpgradeSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _upgradeRequests;

        private readonly GameContext _game;

        public DestroyAbilityEntitiesOnUpgradeSystem(GameContext game)
        {
            _game = game;
            _abilities = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.RecreatedOnUpgrade,
                    GameMatcher.AbilityEnum));

            _upgradeRequests = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.UpgradeRequest,
                    GameMatcher.AbilityEnum));
        }

        public void Execute()
        {
            foreach (GameEntity request in _upgradeRequests)
            foreach (GameEntity ability in _abilities)
            {
                if (request.AbilityEnum != ability.AbilityEnum)
                    continue;

                foreach (GameEntity entity in _game.GetEntitiesWithParentAbility(ability.AbilityEnum))
                    entity.isDestructed = true;

                ability.isActive = false;
            }
        }
    }
}