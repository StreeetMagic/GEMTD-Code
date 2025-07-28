using System;
using System.Collections.Generic;
using System.Linq;
using Tools.MazeDesigner;
using UnityEngine;

namespace Core.Scripts.Tools.MazeDesigner.Editor
{
    public class MazePathCalculator
    {
        /// <summary>
        /// Рассчитывает пути для каждого хода (каждые 5 башен).
        /// Для каждого хода запускается BFS на 6 отрезках пути:
        /// Start->C1, C1->C2, C2->C3, C3->C4, C4->C5, C5->Finish.
        /// Результат записывается в массив mazeData.Turns.
        /// </summary>
        public void CalculateAllTurnsPaths(MazeDataSO mazeData)
        {
            if (mazeData == null)
                throw new Exception("Ссылка на MazeDataSO не установлена.");

            List<MazeCell> allCells = mazeData.MazeCells;
            if (allCells == null || allCells.Count == 0)
                throw new Exception("Список ячеек лабиринта пуст.");

            // --- Находим все ключевые точки (Start, Finish, Checkpoints) ---
            MazeCell startCell = allCells.Find(cell => cell.CellEnum == CellEnum.Start);
            MazeCell finishCell = allCells.Find(cell => cell.CellEnum == CellEnum.Finish);
            MazeCell cp1 = allCells.Find(cell => cell.CellEnum == CellEnum.Checkpoint1);
            MazeCell cp2 = allCells.Find(cell => cell.CellEnum == CellEnum.Checkpoint2);
            MazeCell cp3 = allCells.Find(cell => cell.CellEnum == CellEnum.Checkpoint3);
            MazeCell cp4 = allCells.Find(cell => cell.CellEnum == CellEnum.Checkpoint4);
            MazeCell cp5 = allCells.Find(cell => cell.CellEnum == CellEnum.Checkpoint5);

            if (startCell == null) throw new Exception("Не найдена ячейка Start.");
            if (finishCell == null) throw new Exception("Не найдена ячейка Finish.");
            if (cp1 == null) throw new Exception("Не найден Checkpoint1.");
            if (cp2 == null) throw new Exception("Не найден Checkpoint2.");
            if (cp3 == null) throw new Exception("Не найден Checkpoint3.");
            if (cp4 == null) throw new Exception("Не найден Checkpoint4.");
            if (cp5 == null) throw new Exception("Не найден Checkpoint5.");

            // --- Сегменты, которые нужно просчитывать ---
            // Для удобства храним в массиве, чтобы не писать 6 раз подряд одинаковый код
            var segments = new (Vector2Int from, Vector2Int to)[]
            {
                (startCell.Position, cp1.Position),
                (cp1.Position, cp2.Position),
                (cp2.Position, cp3.Position),
                (cp3.Position, cp4.Position),
                (cp4.Position, cp5.Position),
                (cp5.Position, finishCell.Position)
            };

            // --- Отбираем все башни (CellType.IndexesWall или CellType.StartWall) и сортируем по Order ---
            // Предполагается, что "потенциальные" башни уже в CellType.Wall, но мы будем
            // временно выключать те, чей Order ещё не наступил (делать их Empty при BFS).
            List<MazeCell> towerCells = allCells
                .Where(cell => cell.CellEnum == CellEnum.IndexesWall)
                .OrderBy(cell => cell.Order)
                .ToList();
            
            // Сколько всего ходов (каждые 5 башен = 1 ход)
            int turnCount = (int)Math.Ceiling(towerCells.Count / 5f);

            // Создаём массив для результатов
            mazeData.Rounds = new RoundPathSetup[turnCount];

            // --- Перебираем ходы ---
            for (int turnIndex = 0; turnIndex < turnCount; turnIndex++)
            {
                // Сколько башен "активно" на этом ходу?
                // Например, если turnIndex=0 (первый ход) -> активны башни с Order среди первых 5.
                // turnIndex=1 (второй ход) -> первые 10, и т.д.
                int activeWallCount = (turnIndex + 1) * 5;

                // Копируем оригинальный список клеток, но для "неактивных" стен (тех, у кого Order > activeWallCount)
                // временно меняем тип на Empty (чтобы BFS их мог проходить).
                // Также учтём, что если в лабиринте есть стены, не связанные с Order (например, изначальные),
                // они всегда остаются стенами.
                Dictionary<Vector2Int, MazeCell> turnMazeDict = new Dictionary<Vector2Int, MazeCell>(allCells.Count);

                foreach (var cell in allCells)
                {
                    // Если это башня с Order > активного - делаем её пустой (т.е. проходимой)
                    if (cell.CellEnum == CellEnum.IndexesWall && cell.Order > activeWallCount)
                    {
                        // Создаём копию с типом Empty
                        var modified = new MazeCell(CellEnum.Empty, cell.Order, cell.Position);
                        turnMazeDict[cell.Position] = modified;
                    }
                    else
                    {
                        // Иначе оставляем как есть
                        turnMazeDict[cell.Position] = cell;
                    }
                }

                // Готовим структуру для хранения результатов 6 сегментов
                RoundPathSetup roundResult = new RoundPathSetup();

                // Последовательно ищем путь для каждого из 6 сегментов
                for (int segIndex = 0; segIndex < segments.Length; segIndex++)
                {
                    var (fromPos, toPos) = segments[segIndex];
                    List<Vector2Int> path = Bfs(fromPos, toPos, turnMazeDict);

                    if (path == null || path.Count == 0)
                    {
                        throw new Exception(
                            $"Маршрут не найден на ходу {turnIndex + 1} (после установки {activeWallCount} башен) " +
                            $"для участка {fromPos} -> {toPos}."
                        );
                    }

                    // Сохраняем путь в нужное поле TurnMoveSetup
                    switch (segIndex)
                    {
                        case 0: roundResult.StartToCheckPoint1 = path.ToArray(); break;
                        case 1: roundResult.CheckPoint1ToCheckPoint2 = path.ToArray(); break;
                        case 2: roundResult.CheckPoint2ToCheckPoint3 = path.ToArray(); break;
                        case 3: roundResult.CheckPoint3ToCheckPoint4 = path.ToArray(); break;
                        case 4: roundResult.CheckPoint4ToCheckPoint5 = path.ToArray(); break;
                        case 5: roundResult.CheckPoint5ToFinish = path.ToArray(); break;
                    }
                }

                // Записываем результат в общий массив
                mazeData.Rounds[turnIndex] = roundResult;
            }

            // По итогу в mazeData.Turns будут лежать пути для каждого хода.
        }

        /// <summary>
        /// Поиск пути (BFS) с учётом диагоналей (без проскальзывания).
        /// </summary>
        private List<Vector2Int> Bfs(Vector2Int start, Vector2Int target, Dictionary<Vector2Int, MazeCell> mazeDict)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int?> cameFrom = new Dictionary<Vector2Int, Vector2Int?>();

            queue.Enqueue(start);
            cameFrom[start] = null;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                if (current == target)
                {
                    return ReconstructPath(cameFrom, current);
                }

                // Перебираем все 8 направлений
                foreach (var dir in GetDirections())
                {
                    Vector2Int neighbor = current + dir;

                    if (!mazeDict.TryGetValue(neighbor, out MazeCell neighborCell))
                        continue; // Нет такой клетки

                    // Если сосед - стена, пропускаем
                    if (neighborCell.CellEnum == CellEnum.IndexesWall || neighborCell.CellEnum == CellEnum.StartWall)
                        continue;

                    // Проверяем «проскальзывание» через угол (только для диагоналей)
                    if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 2)
                    {
                        // Пример: dir = (1, 1)
                        // Проверяем ячейки (current.x + 1, current.y) и (current.x, current.y + 1)
                        Vector2Int check1 = new Vector2Int(current.x + dir.x, current.y);
                        Vector2Int check2 = new Vector2Int(current.x, current.y + dir.y);

                        if (mazeDict.TryGetValue(check1, out MazeCell c1) &&
                            (c1.CellEnum == CellEnum.IndexesWall || c1.CellEnum == CellEnum.StartWall))
                            continue;
                        if (mazeDict.TryGetValue(check2, out MazeCell c2) &&
                            (c2.CellEnum == CellEnum.IndexesWall || c2.CellEnum == CellEnum.StartWall))
                            continue;
                    }

                    if (!cameFrom.ContainsKey(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }

            // Если путь не найден
            return null;
        }

        /// <summary>
        /// Восстанавливает путь (список позиций) по словарю предшественников cameFrom.
        /// </summary>
        private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int?> cameFrom, Vector2Int end)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Vector2Int? current = end;

            while (current != null)
            {
                path.Add(current.Value);
                current = cameFrom[current.Value];
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// 8 направлений (4 прямых + 4 диагональных).
        /// </summary>
        private static readonly Vector2Int[] s_directions = new Vector2Int[]
        {
            new Vector2Int(0, 1), // Вверх
            new Vector2Int(0, -1), // Вниз
            new Vector2Int(-1, 0), // Влево
            new Vector2Int(1, 0), // Вправо
            new Vector2Int(1, 1), // Диагональ ↘
            new Vector2Int(1, -1), // Диагональ ↗
            new Vector2Int(-1, 1), // Диагональ ↙
            new Vector2Int(-1, -1), // Диагональ ↖
        };

        private IEnumerable<Vector2Int> GetDirections()
        {
            foreach (var d in s_directions)
            {
                yield return d;
            }
        }
    }
}