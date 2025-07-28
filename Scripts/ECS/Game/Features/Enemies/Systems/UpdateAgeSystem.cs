using Entitas;
using UnityEngine;

namespace Game.Enemies
{
    public class UpdateAgeSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public UpdateAgeSystem(GameContext context)
        {
            _entities = context.GetGroup(GameMatcher.AllOf(GameMatcher.Age));
        }

        public void Execute()
        {
            foreach (var e in _entities)
            {
                e.ReplaceAge(e.Age + Time.deltaTime);
            }
        }
    }
}