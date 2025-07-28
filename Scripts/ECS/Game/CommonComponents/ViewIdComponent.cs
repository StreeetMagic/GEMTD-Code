using Entitas;
using Infrastructure.Prefabs;

namespace Game.CommonComponents
{
    [Game]
    public class ViewIdComponent : IComponent
    {
        public PrefabEnum Value;
    }
}