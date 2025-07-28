using Entitas;
using Services.MazeBuilders;
using Services.ProjectData;
using UnityEngine;

namespace Game.GameMainFeature
{
    public class InitializeGameLoopSystem : IInitializeSystem
    {
        private readonly GameEntityFactories _factories;
        private readonly IProjectDataService _projectDataService;
        private readonly IMazeBuilder _mazeBuilder;

        public InitializeGameLoopSystem(
            GameEntityFactories factories,
            IProjectDataService projectDataService,
            IMazeBuilder mazeBuilder
        )
        {
            _factories = factories;
            _projectDataService = projectDataService;
            _mazeBuilder = mazeBuilder;
        }

        public void Initialize()
        {
            for (var i = 0; i < _projectDataService.StartPositions.Count; i++)
            {
                bool isHuman = i == 0;

                var player = _factories.CreatePlayer(isHuman, i);
                Vector2 startPosition = _projectDataService.StartPositions[i];
                _mazeBuilder.Build(_projectDataService.CurrentMazeData, i, startPosition, player.Id, isHuman);
            }

            _factories.CreateCursor();
            _factories.CreateMainEntity();
        }
    }
}