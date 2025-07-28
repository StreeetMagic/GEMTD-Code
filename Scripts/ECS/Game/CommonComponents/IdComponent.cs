using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.CommonComponents
{
    [Game]
    public class IdComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int Value;
    }
}