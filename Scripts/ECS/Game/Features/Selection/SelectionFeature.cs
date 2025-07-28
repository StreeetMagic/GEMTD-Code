using Game.Highlight;
using Game.Selection.Systems;
using Services.SystemsFactoryServices;

namespace Game.Selection
{
    public sealed class SelectionFeature : Feature
    {
        public SelectionFeature(ISystemFactory systems)
        {
            Add(systems.Create<SelectionSystem>());
            Add(systems.Create<UnSelectionSystem>());
            Add(systems.Create<CancelSelectionByRequestSystem>());
            Add(systems.Create<CancelSelectionByEmptyClickSystem>());
        }
    }
}