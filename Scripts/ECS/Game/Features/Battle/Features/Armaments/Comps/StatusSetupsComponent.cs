using System.Collections.Generic;
using Entitas;

namespace Game.Battle
{
    [Game]
    public class StatusSetupsComponent : IComponent
    {
        public List<StatusSetup> Value;
    }
}