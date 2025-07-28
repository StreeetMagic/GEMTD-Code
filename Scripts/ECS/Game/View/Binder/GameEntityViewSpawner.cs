using Game.Factory;
using UnityEngine;

namespace Game.Binder
{
    public class GameEntityViewSpawner : IGameEntityViewSpawner
    {
        private readonly IGameEntityViewFactory _factory;

        public GameEntityViewSpawner(
            IGameEntityViewFactory factory
        )
        {
            _factory = factory;
        }

        public void Spawn(GameEntity entity, Vector3 at)
        {
            _factory.Create(entity, at);
        }
    }
}