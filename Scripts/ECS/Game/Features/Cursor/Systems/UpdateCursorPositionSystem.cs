using Entitas;
using UnityEngine;

namespace Game.Cursor
{
    public class UpdateCursorPositionSystem : IExecuteSystem
    {
        private readonly GameContext _game;

        public UpdateCursorPositionSystem(GameContext game)
        {
            _game = game;
        }

        public void Execute()
        {
            Vector2 cursorPosition = Input.mousePosition;

            _game.cursorEntity.ReplaceCursorPosition(cursorPosition);
        }
    }
}