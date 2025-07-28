using Game.Battle.EffectApplication;
using Services.SystemsFactoryServices;

namespace Game.Battle
{
    public sealed class BattleFeature : Feature
    {
        public BattleFeature(ISystemFactory systems)
        {
            Add(systems.Create<AbilityFeature>());
            Add(systems.Create<ArmamentFeature>());
            Add(systems.Create<CooldownSystem>());
            Add(systems.Create<EffectFeature>());
            Add(systems.Create<EffectApplicationFeature>());
            Add(systems.Create<StatsFeature>());
            Add(systems.Create<StatusFeature>());
            Add(systems.Create<MovementFeature>());
            
            Add(systems.Create<CleanEmptyTargetsSystem>());
        }
    }
}