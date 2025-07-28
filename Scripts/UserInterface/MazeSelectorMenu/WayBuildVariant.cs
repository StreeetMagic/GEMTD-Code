using System;
using Tools.MazeDesigner;
using UnityEngine;

namespace UserInterface.MazeSelectorMenu
{
    [Serializable]
    public class WayBuildVariant
    {
        public Sprite Sprite;
        public MazeDataSO Maze;
        public bool IsReady;
    }
}