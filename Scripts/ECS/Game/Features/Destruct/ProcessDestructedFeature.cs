using Services.SystemsFactoryServices;

namespace Game.Destruct
{
    public sealed class ProcessDestructedFeature : Feature
    {
        public ProcessDestructedFeature(ISystemFactory systems)
        {
            Add(systems.Create<SelfDestructTimerSystem>());

            Add(systems.Create<CleanupGameDestructedViewSystem>());
            Add(systems.Create<CleanupGameDestructedSystem>());
        }
    }
}