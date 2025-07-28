using System;
using System.Collections.Generic;
using System.Linq;
using Tools.MazeDesigner;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
// ReSharper disable All

namespace Core.Scripts.Tools.MazeDesigner.Editor
{
    public class MazeEditorWindow : EditorWindow
    {
        private const int СellSize = 30;
        private const int MaxCheckPointCount = 5;
        private MazeDataSO _mazeDataSOFromProject;
        private int _width = 10;
        private int _height = 10;
        private CellEnum _selectedCellEnum = CellEnum.Empty;
        private MazeCell[,] _mazeGrid;
        private Vector2 _scrollPosition;
        private bool _mazeDataSOFromProjectIsLoaded;
        private bool _isDrawing;
        //private bool _isErasing;
        private string _newSoName;
        private MazeSizeEnum _mazeSizeEnum;
        
        [MenuItem("Tools/Maze Editor")]
        public static void OpenWindow()
        {
            GetWindow<MazeEditorWindow>("Maze Editor");
        }

        private void OnEnable()
        {
            InitMaze();
            
            _mazeSizeEnum = MazeSizeEnum.Small;
            _width = 17;
            _height = 17;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Maze Editor", EditorStyles.boldLabel);

            HandleInputFromEditor();

            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Reset Maze"))
                InitMaze();

            if (GUILayout.Button("Установить все чекпоинты"))
                SetAllCheckPoints();
            
            EditorGUILayout.EndHorizontal();

            ShowCellButtons();
            ShowCellColors();

            EditorGUILayout.LabelField("Перетащите ScriptableObject Maze сюда:", EditorStyles.boldLabel);
            Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Перетащите SO сюда", EditorStyles.helpBox);
            HandleDragAndDrop(dropArea);

            ReadSOAndFillMatrix();
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            DrawGrid();
            EditorGUILayout.EndScrollView();

            _newSoName = EditorGUILayout.TextField("Имя сохраняемого maze", _newSoName);
            if (GUILayout.Button("Save Maze"))
                SaveMaze(_newSoName);
        }

        private void SetAllCheckPoints()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (_mazeGrid[x, y].CellEnum is CellEnum.Checkpoint1
                        or CellEnum.Checkpoint2
                        or CellEnum.Checkpoint3
                        or CellEnum.Checkpoint4
                        or CellEnum.Checkpoint5
                        or CellEnum.Start
                        or CellEnum.Finish)
                    {
                        _mazeGrid[x, y].CellEnum = CellEnum.Empty;
                    }
                }
            }

            // Координаты чекпоинтов, старта и финиша для каждого масштаба
            List<Vector2Int> checkpointPositions = new();
            Vector2Int startPosition = Vector2Int.zero;
            Vector2Int finishPosition = Vector2Int.zero;

            if (_mazeSizeEnum == MazeSizeEnum.Small)
            {
                checkpointPositions = new List<Vector2Int>
                {
                    new Vector2Int(2, 9),
                    new Vector2Int(14, 9),
                    new Vector2Int(14, 14),
                    new Vector2Int(8, 14),
                    new Vector2Int(8, 2)
                };
                
                startPosition = new Vector2Int(1, 15);
                finishPosition = new Vector2Int(15, 1);
            }
            else if (_mazeSizeEnum == MazeSizeEnum.Big)
            {
                checkpointPositions = new List<Vector2Int>
                {
                    new Vector2Int(3, 18),
                    new Vector2Int(34, 18),
                    new Vector2Int(34, 34),
                    new Vector2Int(18, 34), 
                    new Vector2Int(18, 3)
                };
                
                startPosition = new Vector2Int(1, 35);
                finishPosition = new Vector2Int(35, 3);
            }

            for (int i = 0; i < checkpointPositions.Count && i < MaxCheckPointCount; i++)
            {
                Vector2Int pos = checkpointPositions[i];
                if (pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height)
                {
                    _mazeGrid[pos.x, pos.y].CellEnum = CellEnum.Checkpoint1 + i;
                }
            }

            if (startPosition.x >= 0 && startPosition.x < _width && startPosition.y >= 0 && startPosition.y < _height)
                _mazeGrid[startPosition.x, startPosition.y].CellEnum = CellEnum.Start;

            if (finishPosition.x >= 0 && finishPosition.x < _width && finishPosition.y >= 0 && finishPosition.y < _height)
                _mazeGrid[finishPosition.x, finishPosition.y].CellEnum = CellEnum.Finish;
        }

        private void UpdateMaze()
        {
            if (_width != _mazeGrid.GetLength(0) || _height != _mazeGrid.GetLength(1))
            {
                MazeCell[,] tempMazeGrid = new MazeCell[_width, _height];
                for (int x = 0; x < _width; x++)
                {
                    for (int y = 0; y < _height; y++)
                    {
                        if (_mazeGrid != null &&
                            (_mazeGrid.GetLength(1) <= y || _mazeGrid.GetLength(0) <= x || _mazeGrid[x, y] == null))
                            tempMazeGrid[x, y] = new MazeCell(CellEnum.Empty, 0, new Vector2Int(x, y));
                        else
                            tempMazeGrid[x, y] = _mazeGrid[x, y];
                    }
                }
                _mazeGrid = tempMazeGrid;
            }
        }

        private void ReadSOAndFillMatrix()
        {
            if (_mazeDataSOFromProject != null && _mazeDataSOFromProjectIsLoaded == false)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Выбранный объект:", EditorStyles.boldLabel);
                EditorGUILayout.ObjectField("ScriptableObject", _mazeDataSOFromProject, typeof(MazeDataSO), false);
                int width = _mazeDataSOFromProject.Width;
                int height = _mazeDataSOFromProject.Height;
                _mazeGrid = new MazeCell[width, height];
                foreach (MazeCell cell in _mazeDataSOFromProject.MazeCells)
                {
                    _mazeGrid[cell.Position.x, cell.Position.y] = cell;
                }
                _mazeDataSOFromProjectIsLoaded = true;
            }
        }

        private void ShowCellButtons()
        {
            EditorGUILayout.BeginHorizontal();
            
            foreach (CellEnum cellType in Enum.GetValues(typeof(CellEnum)))
            {
                if (cellType == CellEnum.Unknown)
                    continue;
                GUI.backgroundColor = (cellType == _selectedCellEnum) ? Color.magenta : Color.white;
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
                if (cellType == _selectedCellEnum)
                {
                    buttonStyle.fontStyle = FontStyle.Bold;
                    buttonStyle.normal.textColor = Color.white;
                    buttonStyle.fontSize = 15;
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(cellType.ToString(), buttonStyle, GUILayout.Height(30), GUILayout.Width(100)))
                    _selectedCellEnum = cellType;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowCellColors()
        {
            EditorGUILayout.BeginHorizontal();
            foreach (CellEnum cellType in Enum.GetValues(typeof(CellEnum)).Cast<CellEnum>())
            {
                if (cellType == CellEnum.Unknown)
                    continue;
                GUILayout.FlexibleSpace();
                Rect colorRect = GUILayoutUtility.GetRect(10, 25, GUILayout.Height(20), GUILayout.Width(40));
                EditorGUI.DrawRect(colorRect, GetColorForCell(cellType));
            }
            EditorGUILayout.EndHorizontal();
        }

        private void HandleInputFromEditor()
        {
            EditorGUILayout.LabelField("Настройка масштаба", EditorStyles.boldLabel);
            _mazeSizeEnum = (MazeSizeEnum)EditorGUILayout.EnumPopup("Height", _mazeSizeEnum);
            EditorGUILayout.LabelField("Выбран масштаб: " + _mazeSizeEnum);

            switch (_mazeSizeEnum)
            {
                case MazeSizeEnum.Small:
                    _width = 17;
                    _height = 17;
                    break;

                case MazeSizeEnum.Big:
                    _width = 37;
                    _height = 37;
                    
                    break;
            }
            
            UpdateMaze();
        }

        private int GetCheckpointCountInMaze()
        {
            return _mazeGrid.Cast<MazeCell>().Count(cell =>
                cell.CellEnum is CellEnum.Checkpoint1 or CellEnum.Checkpoint2 or CellEnum.Checkpoint3
                or CellEnum.Checkpoint4 or CellEnum.Checkpoint5);
        }

        private void InitMaze()
        {
            _mazeDataSOFromProjectIsLoaded = false;
            _mazeDataSOFromProject = null;
            _mazeGrid = new MazeCell[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _mazeGrid[x, y] = new MazeCell(CellEnum.Empty, 0, new Vector2Int(x, y));
                }
            }
        }

        private void DrawGrid()
        {
            Event e = Event.current;
            for (int y = _mazeGrid.GetLength(1) - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < _mazeGrid.GetLength(0); x++)
                {
                    Color color = GetColorForCell(_mazeGrid[x, y].CellEnum);
                    GUI.backgroundColor = color;
                    if (GUILayout.Button(GetCellNameInMaze(x, y), GUILayout.Width(СellSize), GUILayout.Height(СellSize)))
                    {
                        if (CanDrawCheckPointCell())
                        {
                            if (e.button == 0)
                            {
                                _mazeGrid[x, y].CellEnum = _selectedCellEnum;
                                if (_selectedCellEnum == CellEnum.IndexesWall)
                                    RecalculateWallOrder(x, y);
                            }
                            else if (e.button == 1)
                            {
                                //_isErasing = true;
                                if (_mazeGrid[x, y].CellEnum == CellEnum.IndexesWall)
                                {
                                    _mazeGrid[x, y].CellEnum = CellEnum.Empty;
                                    _mazeGrid[x, y].Order = 0;
                                    RecalculateAllWalls();
                                }
                                else
                                    _mazeGrid[x, y].CellEnum = CellEnum.Empty;
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private string GetCellNameInMaze(int x, int y)
        {
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

        private void RecalculateWallOrder(int x, int y)
        {
            int maxOrder = _mazeGrid.Cast<MazeCell>()
                .Where(cell => cell != null && cell.CellEnum == CellEnum.IndexesWall)
                .Select(cell => cell.Order)
                .DefaultIfEmpty(0)
                .Max();
            _mazeGrid[x, y].Order = maxOrder + 1;
            Debug.Log("Новая стена, Order = " + _mazeGrid[x, y].Order);
        }

        private bool CanDrawCheckPointCell()
        {
            return _selectedCellEnum switch
            {
                CellEnum.Checkpoint1 or CellEnum.Checkpoint2 or CellEnum.Checkpoint3
                or CellEnum.Checkpoint4 or CellEnum.Checkpoint5 =>
                    GetCheckpointCountInMaze() < MaxCheckPointCount,
                _ => true
            };
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
                case CellEnum.Checkpoint1:
                case CellEnum.Checkpoint2:
                case CellEnum.Checkpoint3:
                case CellEnum.Checkpoint4:
                case CellEnum.Checkpoint5: return Color.yellow;
                default: return Color.blue;
            }
        }

        private void SaveMaze(string newSoName)
        {
            MazeDataSO mazeData = CreateInstance<MazeDataSO>();
            mazeData.Width = _width;
            mazeData.Height = _height;
            mazeData.MazeCells = _mazeGrid.Cast<MazeCell>().ToList();

            List<Vector2Int> towersOrder = new List<Vector2Int>();
            var towerCells = _mazeGrid.Cast<MazeCell>()
                                      .Where(cell => cell != null && cell.CellEnum == CellEnum.IndexesWall)
                                      .OrderBy(cell => cell.Order)
                                      .ToList();
            
            foreach (var towerCell in towerCells)
            {
                towersOrder.Add(towerCell.Position);
            }
            
            mazeData.TowerOrder = towersOrder.ToArray();
            
            MazePathCalculator mazePathCalculator = new MazePathCalculator();
            mazePathCalculator.CalculateAllTurnsPaths(mazeData);
            
            string baseName = "NewMaze.asset";
            string path = "Assets/Core/Resources/Mazes/";
            
            if (String.IsNullOrEmpty(newSoName))
            {
                path += baseName;
            }
            else
            {
                newSoName += ".asset";
                path += newSoName;
            }
            
            AssetDatabase.CreateAsset(mazeData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            ShowNotification(new GUIContent("ScriptableObject сохранен!"));
        }

        private void HandleDragAndDrop(Rect dropArea)
        {
            Event evt = Event.current;
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            if (draggedObject is MazeDataSO mazeDataSOFromProject)
                            {
                                _mazeDataSOFromProject = mazeDataSOFromProject;
                                Repaint();
                            }
                        }
                    }
                    Event.current.Use();
                    break;
            }
        }

        private void RecalculateAllWalls()
        {
            var walls = _mazeGrid.Cast<MazeCell>()
                .Where(cell => cell != null && cell.CellEnum == CellEnum.IndexesWall)
                .OrderBy(cell => cell.Order)
                .ToList();
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].Order = i + 1;
            }
        }
    }
}
