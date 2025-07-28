using Services.SystemsFactoryServices;

namespace Game.Cheats
{
    public sealed class CheatFeature : Feature
    {
        public CheatFeature(ISystemFactory systems)
        {
            Add(systems.Create<CheatSystem>());
        }
    }
}