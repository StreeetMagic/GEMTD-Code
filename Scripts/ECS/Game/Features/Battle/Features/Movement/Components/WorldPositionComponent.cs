using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Game.Battle
{
    [Game]
    [Event(EventTarget.Self)]
    public class WorldPositionComponent : IComponent
    {
        public Vector3 Value;
    }
}