using System.Collections.Generic;
using Entitas;

namespace Game.Towers.MergeSpirits.Comps
{
    [Game]
    public class MergeVariantsComponent : IComponent
    {
        public List<TowerEnum> Value;
    }
}