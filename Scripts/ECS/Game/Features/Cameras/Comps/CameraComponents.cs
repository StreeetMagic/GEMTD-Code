using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Game.Cameras
{
    [Game]
    [Unique]
    public class CameraComponent : IComponent
    {
        public Camera Value;
    }
}