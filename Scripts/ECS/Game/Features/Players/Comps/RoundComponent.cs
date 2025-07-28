using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game
{
    [Game]
    [Event(EventTarget.Self)]
    public class RoundComponent : IComponent
    {
        public int Value;
    }
}