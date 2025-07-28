using Game.Towers.MergeSpirits.Systems;
using Game.Towers.SelectSpirits.Systems;
using Services.SystemsFactoryServices;

namespace Game.Towers
{
    public sealed class TowerFeature : Feature
    {
        public TowerFeature(ISystemFactory systems)
        {
            Add(systems.Create<RotateTowerSystem>());
            
            Add(systems.Create<SelectSpiritSystem>());
            
            Add(systems.Create<SpiritPlacedReactiveSystem>());

            MergeSystems(systems);
        }

        private void MergeSystems(ISystemFactory systems)
        {
            Add(systems.Create<MarkMergeableSpiritsSystem>());
            Add(systems.Create<FilterMergeableSpiritsSystem>());
            Add(systems.Create<FinilizeMergeSpiritsSystem>());
        }
    }
}