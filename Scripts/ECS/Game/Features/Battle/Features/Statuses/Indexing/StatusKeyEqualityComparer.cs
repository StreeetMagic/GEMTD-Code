using System;
using System.Collections.Generic;

namespace Game.Battle
{
    public class StatusKeyEqualityComparer : IEqualityComparer<StatusKey>
    {
        public bool Equals(StatusKey x, StatusKey y)
        {
            return x.TargetId == y.TargetId && x.Enum == y.Enum;
        }

        public int GetHashCode(StatusKey obj)
        {
            return HashCode.Combine(obj.TargetId, (int)obj.Enum);
        }
    }
}