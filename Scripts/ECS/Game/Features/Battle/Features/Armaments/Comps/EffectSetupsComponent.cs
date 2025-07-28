using System.Collections.Generic;
using Entitas;

namespace Game.Battle
{
    [Game]
    public class EffectSetupsComponent : IComponent
    {
        public List<EffectSetup> Value;
    }
}