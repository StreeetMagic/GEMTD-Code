using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.Highlight
{
    [Game]
    [Event(EventTarget.Self, EventType.Added)]
    [Event(EventTarget.Self, EventType.Removed)]
    public class HighlightedComponent : IComponent
    {
    }
}