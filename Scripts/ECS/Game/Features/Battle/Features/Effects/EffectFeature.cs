using Services.SystemsFactoryServices;

namespace Game.Battle
{
    public sealed class EffectFeature : Feature
    {
        public EffectFeature(ISystemFactory systems)
        {
            Add(systems.Create<RemoveEffectsWithoutTargetsSystem>());

            Add(systems.Create<ProcessDamageEffectSystem>());
            Add(systems.Create<ProcessHealEffectSystem>());

            Add(systems.Create<CleanupProcessedEffects>());
        }
    }
}