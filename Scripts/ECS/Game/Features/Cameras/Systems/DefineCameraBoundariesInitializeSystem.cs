using Entitas;
using Services.ProjectData;
using Services.StaticData;

namespace Game.Cameras
{
    public class DefineCameraBoundariesInitializeSystem : IInitializeSystem
    {
        private readonly IGroup<GameEntity> _cameras;
        private readonly IStaticDataService _config;
        private readonly IProjectDataService _projectData;

        public DefineCameraBoundariesInitializeSystem(GameContext game, IStaticDataService config,
            IProjectDataService projectData)
        {
            _config = config;
            _projectData = projectData;

            _cameras = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Camera,
                    GameMatcher.CameraBounds,
                    GameMatcher.CameraTarget));
        }

        public void Initialize()
        {
            var typeMap = _projectData.CurrentGameModeType;

            foreach (var camera in _cameras)
            {
                camera.ReplaceCameraBounds(_config.ProjectConfig.CameraConfig.TryGetBounds(typeMap));
            }
        }
    }
}