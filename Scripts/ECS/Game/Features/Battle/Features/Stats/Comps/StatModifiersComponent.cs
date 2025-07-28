using System.Collections.Generic;
using Entitas;

namespace Game.Battle
{
    [Game]
    public class StatModifiersComponent : IComponent
    {
        public Dictionary<StatEnum, float> Value; //TODO: remove
    }
}