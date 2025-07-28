using Entitas;
using UnityEngine;

namespace Game.Cameras
{
    [Game]
    public class CameraTargetComponent : IComponent
    {
        public Transform Value;
    }
}