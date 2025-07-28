using Entitas;
using Entitas.CodeGeneration.Attributes;
using Unity.Cinemachine;

namespace Game.Cameras
{
    [Game]
    [Unique]
    public class CinemachineCameraComponent : IComponent
    {
        public CinemachineCamera Value;
    }
}