using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.GameMainFeature
{
    [Game]
    [Event(EventTarget.Self)]
    public sealed class TotalGameTimeComponent : IComponent
    {
        public float Value;
    }
}