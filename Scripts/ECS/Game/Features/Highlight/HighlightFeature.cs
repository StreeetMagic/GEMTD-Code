using Services.SystemsFactoryServices;

namespace Game.Highlight
{
    public sealed class HighlightFeature : Feature
    {
        public HighlightFeature(ISystemFactory systems)
        {
            Add(systems.Create<HighlightSystem>());
            Add(systems.Create<UnHighlightSystem>());
        }
    }
}