using System;
using UnityEngine;

namespace Tools.MazeDesigner
{
    [Serializable]
    public class RoundPathSetup
    {
        public Vector2Int[] StartToCheckPoint1;
        public Vector2Int[] CheckPoint1ToCheckPoint2;
        public Vector2Int[] CheckPoint2ToCheckPoint3;
        public Vector2Int[] CheckPoint3ToCheckPoint4;
        public Vector2Int[] CheckPoint4ToCheckPoint5;
        public Vector2Int[] CheckPoint5ToFinish;

        public Vector2Int[] GetPathByRoundIndex(int index)
        {
            return index switch
            {
                0 => StartToCheckPoint1,
                1 => CheckPoint1ToCheckPoint2,
                2 => CheckPoint2ToCheckPoint3,
                3 => CheckPoint3ToCheckPoint4,
                4 => CheckPoint4ToCheckPoint5,
                5 => CheckPoint5ToFinish,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }

        public Vector2Int GetPosition(int route, int blockIndex)
        {
            return GetPathByRoundIndex(route)[blockIndex];
        }
    }
}