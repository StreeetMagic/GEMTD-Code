using Entitas;
using Services.StaticData;
using Services.Times;
using UnityEngine;

namespace Game.Cameras
{
    public class MoveCameraSystem : IExecuteSystem
    {
        private readonly ITimeService _time;
        private readonly IStaticDataService _service;
        
        private readonly IGroup<GameEntity> _cameras;

        public MoveCameraSystem(GameContext game, ITimeService time, IStaticDataService service)
        {
            _time = time;
            _service = service;

            _cameras = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.CameraTarget,
                    GameMatcher.CameraInput,
                    GameMatcher.CameraBounds
                    ));
        }

        public void Execute()
        {
            var moveSpeed = _service.ProjectConfig.CameraConfig.MoveSpeed;
            var boundsDamping = _service.ProjectConfig.CameraConfig.Damping;

            foreach (GameEntity camera in _cameras)
            {
                Transform target = camera.CameraTarget;
                var input = camera.cameraInput.Value;
                var bounds = camera.cameraBounds.Value;
                
                Vector3 moveDirection = new Vector3(input.MoveX, 0, input.MoveZ).normalized;
                target.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
                Vector3 currentPosition = target.position;

                Vector3 clampedPosition = new Vector3(
                    Mathf.Clamp(currentPosition.x, bounds.MinWidth, bounds.MaxWidth),
                    currentPosition.y,
                    Mathf.Clamp(currentPosition.z, bounds.MinHeight, bounds.MaxHeight)
                );
                
                target.position = Vector3.Lerp(currentPosition, clampedPosition, Time.deltaTime * boundsDamping); //PullRequest!
            }
        }
    }
}