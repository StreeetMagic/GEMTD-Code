using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.Battle
{
    [Game]
    public class ApplierStatusLinkComponent : IComponent
    {
        [EntityIndex]
        public int Value;
    }
}