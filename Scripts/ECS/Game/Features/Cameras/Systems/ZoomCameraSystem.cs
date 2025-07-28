using Entitas;
using Services.StaticData;
using Services.Times;
using UnityEngine;

namespace Game.Cameras
{
    public class ZoomCameraSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _cameras;

        private readonly IStaticDataService _service;

        public ZoomCameraSystem(GameContext game, ITimeService time, IStaticDataService service)
        {
            _service = service;

            _cameras = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.CameraTarget,
                    GameMatcher.CameraInput,
                    GameMatcher.ZoomState
                )
            );
        }

        public void Execute()
        {
            var zoomSpeed = _service.ProjectConfig.CameraConfig.ZoomSpeed;
            var minHeight = _service.ProjectConfig.CameraConfig.MinHeight;
            var maxHeight = _service.ProjectConfig.CameraConfig.MaxHeight;
            var zoomDamping  = _service.ProjectConfig.CameraConfig.Damping;

            foreach (GameEntity camera in _cameras)
            {
                var target = camera.CameraTarget;
                var input = camera.CameraInput;

                if (!camera.hasZoomState)
                {
                    camera.AddZoomState(target.position.y);
                }
                
                float targetY = camera.zoomState.Value - input.Zoom * zoomSpeed;

                float clampedTargetY = Mathf.Clamp(targetY, minHeight, maxHeight);
                camera.ReplaceZoomState(clampedTargetY);

                Vector3 currentPosition = target.position;
                float newY = Mathf.Lerp(
                    currentPosition.y,
                    clampedTargetY, 
                    zoomDamping * Time.deltaTime
                );

                currentPosition.y = newY;
                target.position = currentPosition;
            }
        }
    }
}