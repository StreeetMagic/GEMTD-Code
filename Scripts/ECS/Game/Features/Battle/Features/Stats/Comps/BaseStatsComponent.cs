using System.Collections.Generic;
using Entitas;

namespace Game.Battle
{
    [Game]
    public class BaseStatsComponent : IComponent
    {
        public Dictionary<StatEnum, float> Value; //TODO: remove
    }
}