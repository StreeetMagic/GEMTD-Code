using Entitas;
using Services.StaticData;
using Services.Times;
using UnityEngine;

namespace Game.Cameras
{
    public class StrafeCameraSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _cameras;

        private readonly IStaticDataService _service;
        private readonly ITimeService _time;

        public StrafeCameraSystem(GameContext game, IStaticDataService service, ITimeService time)
        {
            _service = service;
            _time = time;

            _cameras = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.CameraTarget,
                    GameMatcher.CameraInput
                ));
        }

        public void Execute()
        {
#if UNITY_EDITOR
            if (!IsMouseInsideGameWindow())
                return;
#endif

            var edgeThickness = _service.ProjectConfig.CameraConfig.EdgeThickness;
            var strafeSpeed = _service.ProjectConfig.CameraConfig.StrafeSpeed;

            foreach (GameEntity camera in _cameras)
            {
                Vector3 cameraMove = Vector3.zero;

                if (Input.mousePosition.x <= edgeThickness)
                    cameraMove.x = -strafeSpeed * Time.deltaTime;

                if (Input.mousePosition.x >= Screen.width - edgeThickness)
                    cameraMove.x = strafeSpeed * Time.deltaTime;

                if (Input.mousePosition.y <= edgeThickness)
                    cameraMove.z = -strafeSpeed * Time.deltaTime;

                if (Input.mousePosition.y >= Screen.height - edgeThickness)
                    cameraMove.z = strafeSpeed * Time.deltaTime;

                camera.CameraTarget.Translate(cameraMove, Space.World);
            }
        }

#if UNITY_EDITOR
        private bool IsMouseInsideGameWindow()
        {
            Vector3 pos = Input.mousePosition;
            return pos.x >= 0 && pos.y >= 0 && pos.x <= Screen.width && pos.y <= Screen.height;
        }
#endif
    }
}