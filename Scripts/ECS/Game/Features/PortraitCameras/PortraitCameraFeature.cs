using Game.PortraitCameras.Systems;
using Services.SystemsFactoryServices;

namespace Game.PortraitCameras
{
    public class PortraitCameraFeature : Feature
    {
        public PortraitCameraFeature(ISystemFactory systems)
        {
            Add(systems.Create<MovePortraitCameraSystem>());
            Add(systems.Create<SetPortraitTargetSystem>());
        }
    }
}