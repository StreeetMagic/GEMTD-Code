using Game.Battle.AuraAbility.Systems;
using Game.Battle.BasicAttack.Systems;
using Game.Battle.CleaveAbility.Systems;
using Game.Battle.PierceAbility.Systems;
using Game.Battle.PoisonAbility.Systems;
using Game.Battle.SlowAbility.Systems;
using Services.SystemsFactoryServices;

namespace Game.Battle
{
    public sealed class AbilityFeature : Feature
    {
        public AbilityFeature(ISystemFactory systems)
        {
            Add(systems.Create<AuraTargetSelectionSystem>());
            Add(systems.Create<DestroyAbilityEntitiesOnUpgradeSystem>());

            Add(systems.Create<TowerTargetSelectionSystem>());

            Add(systems.Create<BasicAttackAbilitySystem>());
            Add(systems.Create<SlowAbilitySystem>());
            Add(systems.Create<PoisonAbilitySystem>());
            Add(systems.Create<PierceAbilitySystem>());

            Add(systems.Create<CleaveAbilitySystem>());
        }
    }
}