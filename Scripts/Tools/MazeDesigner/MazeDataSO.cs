using System.Collections.Generic;
using Sirenix.Serialization;
using Unity.Collections;
using UnityEngine;

namespace Tools.MazeDesigner
{
    [CreateAssetMenu(fileName = nameof(MazeDataSO), menuName = "Configs/" + nameof(MazeDataSO))]
    // ReSharper disable once InconsistentNaming
    public class MazeDataSO : ScriptableObject
    {
        [OdinSerialize]
        public List<MazeCell> MazeCells;

        [OdinSerialize]
        public Vector2Int[] TowerOrder;

        [OdinSerialize]
        [ReadOnly]
        public int Width;

        [OdinSerialize]
        [ReadOnly]
        public int Height;

        public RoundPathSetup[] Rounds;
    }
}