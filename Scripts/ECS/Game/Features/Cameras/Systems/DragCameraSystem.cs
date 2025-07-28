using Entitas;
using UnityEngine;

namespace Game.Cameras
{
    public class DragCameraSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _cameras;

        public DragCameraSystem(GameContext game)
        {
            _cameras = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.CameraDrag,
                    GameMatcher.CameraInput,
                    GameMatcher.CameraTarget
                    ));
        }

        public void Execute()
        {
            foreach (GameEntity camera in _cameras)
            {
                Vector3 dragDelta = camera.CameraDrag;
                var transform = camera.CameraTarget;
            
                Vector3 move = new Vector3(dragDelta.x, 0, dragDelta.z);
                transform.Translate(move, Space.World);
                camera.ReplaceCameraDrag(Vector3.zero);
            }
        }
    }
}