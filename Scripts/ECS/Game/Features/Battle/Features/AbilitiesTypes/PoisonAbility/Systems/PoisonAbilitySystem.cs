using Entitas;

namespace Game.Battle.PoisonAbility.Systems
{
    public class PoisonAbilitySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _towers;

        public PoisonAbilitySystem(GameContext game)
        {
            _abilities = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.PoisonAbility,
                    GameMatcher.ProducerId));

            _towers = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.TowerEnum));
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

                tower.isPoisonProjectiles = true;

                ability.PutOnCooldown();
            }
        }
    }
}