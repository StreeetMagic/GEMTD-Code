using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.PlayerAbility.SwapAbility.Comps
{
    namespace Game.PlayerAbility.SwapAbility.Comps
    {
        [Game]
        [Unique]
        public class ConfirmSwapWindowDataComponent : IComponent
        {
            public SwapPair Value;
        }

        public readonly struct SwapPair
        {
            public  readonly GameEntity FirstTower;
            public readonly GameEntity SecondTower;
            
            public SwapPair(GameEntity firstTower, GameEntity secondTower)
            {
                FirstTower = firstTower;
                SecondTower = secondTower;
            }
        }
    }
}