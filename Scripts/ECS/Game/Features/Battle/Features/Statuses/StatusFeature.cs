using Services.SystemsFactoryServices;

namespace Game.Battle
{
    public sealed class StatusFeature : Feature
    {
        public StatusFeature(ISystemFactory systems)
        {
            Add(systems.Create<StatusDurationSystem>());
            Add(systems.Create<PeriodicDamageStatusSystem>());
            Add(systems.Create<ApplyAttackSpeedSystem>());
            Add(systems.Create<ApplyE1AttackSpeedSystem>());
            Add(systems.Create<ApplyE2AttackSpeedSystem>());
            Add(systems.Create<ApplyE3AttackSpeedSystem>());
            Add(systems.Create<ApplyE4AttackSpeedSystem>());
            Add(systems.Create<ApplyE5AttackSpeedSystem>());
            Add(systems.Create<ApplyE6AttackSpeedSystem>());
            Add(systems.Create<ApplyFreezeStatusSystem>());
            Add(systems.Create<ApplyDecreaseArmorStatusSystem>());
            Add(systems.Create<ApplyAdditionalProjectilesSystem>());

            Add(systems.Create<CleanupUnappliedStatusLinkedChanges>());
            Add(systems.Create<CleanupUnappliedStatuses>());
        }
    }
}