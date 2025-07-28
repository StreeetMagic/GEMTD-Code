using Entitas;
using Entitas.CodeGeneration.Attributes;
using Game.Battle;
using Game.Extensions;

namespace Game.PlayerAbility.Comps
{
    [Game]
    [Event(EventTarget.Any)]
    public class AbilityUsingFinishedEventComponent : IComponent
    {
        public AbilityEnum Value;
    }
}