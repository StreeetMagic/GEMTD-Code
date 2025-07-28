using Game.Systems;
using Services.SystemsFactoryServices;

namespace Game
{
    public sealed class BindViewFeature : Feature
    {
        public BindViewFeature(ISystemFactory systems)
        {
            Add(systems.Create<BindEntityViewFromPrefabSystem>());
        }
    }
}