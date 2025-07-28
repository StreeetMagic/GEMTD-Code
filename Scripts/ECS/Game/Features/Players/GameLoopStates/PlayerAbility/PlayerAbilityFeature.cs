using Game.PlayerAbility.HealThroneAbility.Systems;
using Game.PlayerAbility.SwapAbility.Systems;
using Game.PlayerAbility.Systems;
using Game.PlayerAbility.TimeLapseAbility.Systems;
using Services.SystemsFactoryServices;

namespace Game.PlayerAbility
{
    public sealed class PlayerAbilityFeature : Feature
    {
        public PlayerAbilityFeature(ISystemFactory systems)
        {
            Add(systems.Create<PlayerAbilityStateSystem>());
            
            Add(systems.Create<SwapSelectionAbilitySystem>());
            Add(systems.Create<SwapFinishAbilitySystem>());
            Add(systems.Create<DeactivateSwapAbilitySystem>());
            
            Add(systems.Create<HealThroneAbilitySystem>());
            
            Add(systems.Create<TimeLapsePlayerAbilitySystem>());
        }
    }
}