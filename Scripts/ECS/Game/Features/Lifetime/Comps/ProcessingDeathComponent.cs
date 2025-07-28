using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.Lifetime
{
    [Game]
    [Event(EventTarget.Self, EventType.Removed)]
    public class ProcessingDeathComponent : IComponent
    {
    }
}