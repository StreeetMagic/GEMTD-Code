using System.Collections.Generic;
using Entitas;
using Game.Binder;
using UnityEngine;

namespace Game.Systems
{
    public class BindEntityViewFromPrefabSystem : IExecuteSystem
    {
        private readonly IGameEntityViewSpawner _spawner;

        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        public BindEntityViewFromPrefabSystem(
            GameContext game,
            IGameEntityViewSpawner gameEntityViewSpawner
        )
        {
            _spawner = gameEntityViewSpawner;

            _entities =
                game.GetGroup(
                    GameMatcher
                        .AllOf(
                            GameMatcher.Prefab
                        )
                        .NoneOf(
                            GameMatcher.View
                        )
                );
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                Vector3 at = new Vector3(1000, 1000, 1000);

                if (entity.hasWorldPosition)
                    at = entity.WorldPosition;

                _spawner.Spawn(entity, at);
                entity.RemovePrefab();
            }
        }
    }
}