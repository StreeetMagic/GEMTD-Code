using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.CommonComponents
{
    [Game]
    public class EntityLinkComponent : IComponent
    {
        [EntityIndex]
        public int Value;
    }
}