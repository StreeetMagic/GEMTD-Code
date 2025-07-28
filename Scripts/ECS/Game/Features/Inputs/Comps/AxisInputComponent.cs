using Entitas;
using UnityEngine;

namespace Game.Inputs
{
    [Game]
    public class AxisInputComponent : IComponent
    {
        public Vector2 Value;
    }
}