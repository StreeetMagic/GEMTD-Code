using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game
{
    [Game]
    [Event(EventTarget.Self)]
    public class GameLoopStateEnumComponent : IComponent
    {
        public GameLoopStateEnum Value;
    }
}