using Entitas;
using UnityEngine;

namespace Game.Cameras
{
    public class FocusCameraOnPlayerSystem : IInitializeSystem
    {
        private readonly IGroup<GameEntity> _cameras;
        private readonly IGroup<GameEntity> _startPoints;
        private readonly IGroup<GameEntity> _finishPoints;

        public FocusCameraOnPlayerSystem(GameContext game)
        {
            _cameras = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.CameraTarget,
                    GameMatcher.CameraStartPosition
                ));

            _startPoints = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.StartPoint,
                    GameMatcher.WorldPosition,
                    GameMatcher.Human
                ));

            _finishPoints = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.FinishPoint,
                    GameMatcher.WorldPosition,
                    GameMatcher.Human
                ));
        }

        public void Initialize()
        {
            if (_startPoints.count == 0 || _finishPoints.count == 0)
                return;

            Vector3 start = _startPoints.GetSingleEntity().WorldPosition;
            Vector3 finish = _finishPoints.GetSingleEntity().WorldPosition;

            Vector3 center = (start + finish) / 2f;

            foreach (var camera in _cameras)
            {
                camera.ReplaceCameraStartPosition(center);
                camera.CameraTarget.transform.position = center;
            }
        }
    }
}