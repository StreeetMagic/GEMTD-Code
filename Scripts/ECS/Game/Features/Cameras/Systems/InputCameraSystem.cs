using Entitas;
using Services.Cameras;
using UnityEngine;

namespace Game.Cameras
{
    public class InputCameraSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _cameras;
        private readonly ICameraProvider _cameraProvider;
        private Plane _dragPlane;
        private Vector3 _dragAnchor;
        private bool _isDragging;

        public InputCameraSystem(GameContext game, ICameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
            _cameras = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.CameraInput,
                    GameMatcher.CameraDrag,
                    GameMatcher.CameraTarget
                ));
        }

        public void Execute()
        {
            foreach (GameEntity camera in _cameras)
            {
                camera.CameraInput.MoveX = Input.GetAxis(camera.CameraInput.AxisHorizontal);
                camera.CameraInput.MoveZ = Input.GetAxis(camera.CameraInput.AxisVertical);
                camera.CameraInput.Rotate = Input.GetAxis(camera.CameraInput.AxisRotation);
                camera.CameraInput.Zoom = Input.mouseScrollDelta.y * 3f;

                camera.ReplaceCameraInput(camera.CameraInput);

                if (Input.GetMouseButtonDown(2))
                {
                    _dragPlane = new Plane(Vector3.up, Vector3.zero);
                    Ray ray = _cameraProvider.Camera.ScreenPointToRay(Input.mousePosition);
                    if (_dragPlane.Raycast(ray, out float enter))
                    {
                        _dragAnchor = ray.GetPoint(enter);
                        _isDragging = true;
                    }
                }

                if (Input.GetMouseButton(2) && _isDragging)
                {
                    Ray ray = _cameraProvider.Camera.ScreenPointToRay(Input.mousePosition);
                    if (_dragPlane.Raycast(ray, out float enter))
                    {
                        Vector3 currentPoint = ray.GetPoint(enter);
                        Vector3 delta = _dragAnchor - currentPoint;
                        camera.ReplaceCameraDrag(delta);
                    }
                }
                else
                {
                    camera.ReplaceCameraDrag(Vector3.zero);
                }

                if (Input.GetMouseButtonUp(2))
                {
                    _isDragging = false;
                }
            }
        }
    }
}