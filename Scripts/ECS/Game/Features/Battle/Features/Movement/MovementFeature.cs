using Services.SystemsFactoryServices;

namespace Game.Battle
{
    public sealed class MovementFeature : Feature
    {
        public MovementFeature(ISystemFactory systems)
        {
            Add(systems.Create<UpdateWorldPositionSystem>());
            Add(systems.Create<UpdateRotationSystem>());
        }
    }
}