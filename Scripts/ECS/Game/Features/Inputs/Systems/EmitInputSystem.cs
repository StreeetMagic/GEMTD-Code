using Entitas;
using UnityEngine;

namespace Game.Inputs
{
    public class EmitInputSystem : IExecuteSystem
    {
        private readonly GameEntityFactories _factories;

        public EmitInputSystem(GameEntityFactories factories)
        {
            _factories = factories;
        }

        public void Execute()
        {
            if (Input.GetMouseButtonDown(0))
            {
               _factories.CreateLeftMouseClick();
            }
        }
    }
}