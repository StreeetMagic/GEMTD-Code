using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.Battle
{
    [Game]
    public class ParentAbilityComponent : IComponent
    {
        [EntityIndex]
        public AbilityEnum Value;
    }
}