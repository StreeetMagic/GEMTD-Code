using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.Meta
{
    [Game]
    [Event(EventTarget.Self)]
    public class GoldComponent : IComponent
    {
        public int Value;
    }
}