using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.MazeDesigner
{
    [Serializable]
    public class MazeCell
    {
        [FormerlySerializedAs("CellType")]
        [SerializeField] public CellEnum CellEnum;  
        [SerializeField] public Vector2Int Position;  
        [SerializeField] public int Order;

        public MazeCell(CellEnum cellEnum, int order, Vector2Int position)
        {
            CellEnum = cellEnum;
            Order = order;
            Position = position;
        }
    }
}