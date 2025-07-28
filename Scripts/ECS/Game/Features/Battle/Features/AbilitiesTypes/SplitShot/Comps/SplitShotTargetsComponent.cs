using Entitas;
using Game.Battle.SplitShot.Data;

namespace Game.Battle.SplitShot
{
    [Game]
    public class SplitShotTargetsComponent : IComponent
    {
        public TargetDistanceData?[] Value;
    }
}