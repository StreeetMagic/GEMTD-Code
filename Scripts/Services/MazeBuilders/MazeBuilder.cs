using System;
using Game.Maze;
using Services.AssetProviders;
using Services.ViewContainerProviders;
using Tools.MazeDesigner;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services.MazeBuilders
{
    public class MazeBuilder : IMazeBuilder
    {
        private readonly GameEntityFactories _factories;
        private readonly IAssetProvider _assetProvider;
        private readonly ViewContainerProvider _viewContainerProvider;

        public MazeBuilder(
            GameEntityFactories factories,
            IAssetProvider assetProvider,
            ViewContainerProvider viewContainerProvider
        )
        {
            _factories = factories;
            _assetProvider = assetProvider;
            _viewContainerProvider = viewContainerProvider;
        }

        public void Build(
            MazeDataSO mazeData,
            int index,
            Vector2 offsets,
            int playerId,
            bool isHuman
        )
        {
            foreach (var cell in mazeData.MazeCells)
            {
                int mazeX = cell.Position.x;
                int mazeY = cell.Position.y;

                int worldX = (int)offsets.x + mazeX;
                int worldZ = (int)offsets.y + mazeY;

                switch (cell.CellEnum)
                {
                    case CellEnum.Start:
                        _factories.CreateStartPoint(worldX, worldZ, playerId, isHuman);
                        break;

                    case CellEnum.Finish:
                        _factories.CreateFinishPoint(worldX, worldZ, playerId, isHuman);
                        break;

                    case CellEnum.Checkpoint1:
                        _factories.CreateCheckpoint(worldX, worldZ, 1, playerId, isHuman);
                        break;

                    case CellEnum.Checkpoint2:
                        _factories.CreateCheckpoint(worldX, worldZ, 2, playerId, isHuman);
                        break;

                    case CellEnum.Checkpoint3:
                        _factories.CreateCheckpoint(worldX, worldZ, 3, playerId, isHuman);
                        break;

                    case CellEnum.Checkpoint4:
                        _factories.CreateCheckpoint(worldX, worldZ, 4, playerId, isHuman);
                        break;

                    case CellEnum.Checkpoint5:
                        _factories.CreateCheckpoint(worldX, worldZ, 5, playerId, isHuman);
                        break;

                    case CellEnum.StartWall:
                        _factories.CreateWall(worldX, worldZ, mazeX, mazeY, playerId);
                        break;

                    case CellEnum.IndexesWall:
                    case CellEnum.Empty:
                        break;

                    case CellEnum.Unknown:
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var blockEnum = (mazeX + mazeY) % 2 == 0
                    ? BlockEnum.Light
                    : BlockEnum.Dark;

                CreateVisualBlock(worldX, worldZ, blockEnum);
            }
        }

        private void CreateVisualBlock(int worldX, int worldZ, BlockEnum blockEnum)
        {
            GameObject prefab = blockEnum switch
            {
                BlockEnum.Light => _assetProvider.LoadAsset("BlockLight"),
                BlockEnum.Dark => _assetProvider.LoadAsset("BlockDark"),
                _ => throw new ArgumentOutOfRangeException(nameof(blockEnum), blockEnum, null)
            };

            Object.Instantiate(
                prefab,
                new Vector3(worldX, 0, worldZ),
                Quaternion.identity,
                _viewContainerProvider.BlockContainer
            );
        }
    }
}