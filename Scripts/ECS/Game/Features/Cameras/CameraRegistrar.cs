using Game.Registrars;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Game.Cameras
{
    public class CameraRegistrar : EntityComponentRegistrar
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private CinemachineCamera _cameraCinema;

        [SerializeField]
        private Transform _transform;

        [Inject]
        private GameContext _game;

        public override void RegisterComponents()
        {
            Entity.AddCamera(_camera);
            Entity.AddCinemachineCamera(_cameraCinema);
            //  Entity.isCamera = true;
            Entity.AddWorldPosition(transform.position);
            Entity.AddCameraInput(new CameraInputData());
            Entity.AddCameraTarget(_transform);
            Entity.AddCameraDrag(Vector3.zero);
            Entity.AddCameraStartPosition(_transform.position);
            Entity.AddCameraBounds(new CameraBoundsData());
            Entity.AddRay(default);
            Entity.AddZoomState(transform.position.y);
        }

        public override void UnregisterComponents()
        {
            if (Entity.hasCamera)
                Entity.RemoveCamera();

            if (Entity.hasCinemachineCamera)
                Entity.RemoveCinemachineCamera();

            if (Entity.hasCameraTarget)
                Entity.RemoveCameraTarget();

            if (Entity.hasCameraInput)
                Entity.RemoveCameraInput();

            if (Entity.hasCameraDrag)
                Entity.RemoveCameraDrag();

            if (Entity.hasCameraStartPosition)
                Entity.RemoveCameraStartPosition();

            if (Entity.hasCameraBounds)
                Entity.RemoveCameraBounds();

            if (Entity.hasZoomState)
                Entity.RemoveZoomState();

            // if (Entity.isCamera)
            //     Entity.isCamera = false;
        }

        public void Update()
        {
            if (_game.cursorEntity == null)
                return;

            Vector2 cursorPosition = _game.cursorEntity.CursorPosition;

            Ray ray = _camera.ScreenPointToRay(new Vector3(cursorPosition.x, cursorPosition.y, 0f));

            Entity.ReplaceRay(ray);
        }
    }
}