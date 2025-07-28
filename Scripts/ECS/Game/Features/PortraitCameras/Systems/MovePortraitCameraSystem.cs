using Entitas;
using UnityEngine;

namespace Game.PortraitCameras.Systems
{
    public class MovePortraitCameraSystem : IExecuteSystem
    {
        private static readonly Vector3 s_throneScale = new(2f, 2f, 2f);
        private static readonly Vector3 s_default = Vector3.one;

        private readonly IGroup<GameEntity> _portraitTargets;
        private readonly IGroup<GameEntity> _cameras;

        public MovePortraitCameraSystem(GameContext context)
        {
            _portraitTargets = context.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.PortraitTarget,
                        GameMatcher.WorldPosition
                    ));

            _cameras = context.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.PortraitCamera,
                        GameMatcher.WorldPosition,
                        GameMatcher.Scale
                    ));
        }

        public void Execute()
        {
            foreach (var camera in _cameras)
            foreach (var target in _portraitTargets)
            {
                var targetPosition = target.WorldPosition;

                var targetScale = target.isTower 
                    ? s_default 
                    : s_throneScale;

                UpdateCamera(camera, targetPosition, targetScale);
            }
        }

        private static void UpdateCamera(GameEntity camera, Vector3 position, Vector3 scale)
        {
            camera.ReplaceWorldPosition(position);
            camera.ReplaceScale(scale);
        }
    }
}