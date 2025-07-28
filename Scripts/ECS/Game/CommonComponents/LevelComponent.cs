using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.CommonComponents
{
    [Game]
    [Event(EventTarget.Self)]
    public class LevelComponent : IComponent
    {
        public int Value;
    }
}