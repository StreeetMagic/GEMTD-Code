using Services.SystemsFactoryServices;

namespace Game.Cameras
{
    public sealed class CameraFeature : Feature
    {
        public CameraFeature(ISystemFactory systems)
        {
            Add(systems.Create<FocusCameraOnPlayerSystem>());
            Add(systems.Create<InputCameraSystem>());
            Add(systems.Create<MoveCameraSystem>());
            Add(systems.Create<DragCameraSystem>());
            Add(systems.Create<StrafeCameraSystem>());
            Add(systems.Create<ResetCameraSystem>());
            Add(systems.Create<DefineCameraBoundariesInitializeSystem>());
            Add(systems.Create<ZoomCameraSystem>());
        }
    }
}