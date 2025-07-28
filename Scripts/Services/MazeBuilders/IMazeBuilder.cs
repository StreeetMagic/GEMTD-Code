using Tools.MazeDesigner;
using UnityEngine;

namespace Services.MazeBuilders
{
    public interface IMazeBuilder
    {
        public void Build(MazeDataSO mazeDataSo, int index, Vector2 offsets, int playerId, bool isHuman);
    }
}