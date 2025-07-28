using Game.Battle.Game.Features.Battle.Armaments;
using Services.SystemsFactoryServices;

namespace Game.Battle
{
    public sealed class ArmamentFeature : Feature
    {
        public ArmamentFeature(ISystemFactory systems)
        {
            Add(systems.Create<MarkProcessedOnTargetLimitExceededSystem>());
            Add(systems.Create<FollowProducerSystem>());
            Add(systems.Create<UpdateArmamentDirectionSystem>());
            Add(systems.Create<CleaveArmamentHitDetectionSystem>());
            Add(systems.Create<ArmamentHitDetectionSystem>());

            Add(systems.Create<ExplosionReactiveSystem>());
            Add(systems.Create<MuzzleFlashReactiveSystem>());

            Add(systems.Create<FinalizeProcessedArmamentsSystem>());
        }
    }
}