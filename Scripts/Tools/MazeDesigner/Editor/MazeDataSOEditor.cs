using Tools.MazeDesigner;
using UnityEditor;
using UnityEngine;
// ReSharper disable All

namespace Core.Scripts.Tools.MazeDesigner.Editor
{
    [CustomEditor(typeof(MazeDataSO))]
    // ReSharper disable once InconsistentNaming
    public class MazeDataSOEditor : UnityEditor.Editor
    {
        private MazeDataSO mazeData;
        private MazeCell[,] _mazeGrid;

        private void OnEnable()
        {
            mazeData = (MazeDataSO)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (mazeData.MazeCells == null)
            {
                EditorGUILayout.HelpBox("Лабиринт пуст. Сгенерируйте его в MazeEditorWindow.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Визуализация лабиринта:", EditorStyles.boldLabel);
            DrawMazeGrid();
        }

        private void DrawMazeGrid()
        {
            int width = mazeData.Width;
            int height = mazeData.Height;
            _mazeGrid = new MazeCell[width, height];

            foreach (MazeCell cell in mazeData.MazeCells)
            {
                _mazeGrid[cell.Position.x, cell.Position.y] = cell;
            }

            for (int y = height - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();

                for (int x = 0; x < width; x++)
                {
                    MazeCell cell = _mazeGrid[x, y];
                    Color cellColor = cell != null ? GetColorForCell(cell.CellEnum) : Color.gray;
                    string cellName = GetCellNameInMaze(x, y);
                    DrawCell(cellColor, cellName);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private string GetCellNameInMaze(int x, int y)
        {
            if (_mazeGrid == null)
                return "";

            switch (_mazeGrid[x, y].CellEnum)
            {
                case CellEnum.IndexesWall: return _mazeGrid[x, y].Order.ToString();
                case CellEnum.Checkpoint1: return "C1";
                case CellEnum.Checkpoint2: return "C2";
                case CellEnum.Checkpoint3: return "C3";
                case CellEnum.Checkpoint4: return "C4";
                case CellEnum.Checkpoint5: return "C5";
                default: return "";
            }
        }

        private void DrawCell(Color color, string cellName)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTexture(20, 20, color) }
            };

            GUILayout.Box(cellName, style, GUILayout.Width(20), GUILayout.Height(20));
        }

        private Texture2D MakeTexture(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private Color GetColorForCell(CellEnum cellEnum)
        {
            switch (cellEnum)
            {
                case CellEnum.Empty: return Color.white;
                case CellEnum.Start: return Color.green;
                case CellEnum.Finish: return Color.red;
                case CellEnum.StartWall: return Color.grey;
                case CellEnum.IndexesWall: return Color.black;
                case CellEnum.Checkpoint1: return Color.yellow;
                case CellEnum.Checkpoint2: return Color.yellow;
                case CellEnum.Checkpoint3: return Color.yellow;
                case CellEnum.Checkpoint4: return Color.yellow;
                case CellEnum.Checkpoint5: return Color.yellow;
                default: return Color.blue;
            }
        }
    }
}