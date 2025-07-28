using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game
{
    [Game]
    [Event(EventTarget.Self)]
    public sealed class RoundTimerComponent : IComponent
    {
        public float Value;
    }
}