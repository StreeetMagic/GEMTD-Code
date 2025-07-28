using Entitas;
using UnityEngine;

namespace Game.Cameras
{
    public class ResetCameraSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _cameras;

        public ResetCameraSystem(GameContext game)
        {
            _cameras = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.Camera,
                    GameMatcher.CameraTarget,
                    GameMatcher.CameraInput,
                    GameMatcher.CameraStartPosition
                    ));
        }

        public void Execute()
        {
            foreach (GameEntity camera in _cameras)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    camera.CameraTarget.position = camera.CameraStartPosition;
                }
            }
        }
    }
}