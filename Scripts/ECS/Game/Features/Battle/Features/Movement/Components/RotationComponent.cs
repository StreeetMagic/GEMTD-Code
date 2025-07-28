using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Game.Battle
{
    [Game]
    [Event(EventTarget.Self)]
    public class RotationComponent : IComponent
    {
        public Quaternion Value;
    }
}