using Entitas;

namespace Game.Battle.PierceAbility.Systems
{
    public class PierceAbilitySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _towers;

        public PierceAbilitySystem(GameContext game)
        {
            _abilities = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.PierceAbility,
                    GameMatcher.ProducerId));

            _towers = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.TowerEnum));
        }

        public void Execute()
        {
            foreach (GameEntity ability in _abilities)
            foreach (GameEntity tower in _towers)
            {
                if (tower.Id != ability.ProducerId)
                    continue;

                if (!ability.isCooldownUp)
                    continue;

                tower.isPierceProjectiles = true;

                ability.PutOnCooldown();
            }
        }
    }
}