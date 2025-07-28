using Entitas;
using UnityEngine;

namespace Game.Cursor
{
    [Game]
    public class CursorPositionComponent : IComponent
    {
        public Vector2 Value;
    }
}