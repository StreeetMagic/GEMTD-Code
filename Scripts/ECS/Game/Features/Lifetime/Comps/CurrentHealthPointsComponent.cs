using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.Lifetime
{
    [Game]
    [Event(EventTarget.Self)]
    public class CurrentHealthPointsComponent : IComponent
    {
        public float Value;
    }
}