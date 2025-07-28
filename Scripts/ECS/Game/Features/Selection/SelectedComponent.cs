using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.Selection
{
    [Game]
    [Event(EventTarget.Self, EventType.Added)]
    [Event(EventTarget.Self, EventType.Removed)]
    public class SelectedComponent : IComponent
    {
    }
}