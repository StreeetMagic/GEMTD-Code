using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Game.Battle
{
    namespace Game.Features.Battle.Armaments
{
    public class CleaveArmamentHitDetectionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _armaments;
        private readonly GameContext _game;
        private readonly IArmamentFactory _armamentFactory;
        private readonly IGroup<GameEntity> _enemies;
        private List<GameEntity> _buffer = new List<GameEntity>(16);
        private const float CleaveRadius = 0.5f;

        public CleaveArmamentHitDetectionSystem(GameContext game, IArmamentFactory armamentFactory)
        {
            _game = game;
            _armamentFactory = armamentFactory;
            _armaments = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Armament,
                    GameMatcher.WorldPosition,
                    GameMatcher.TargetBuffer,
                    GameMatcher.CleaveArmament,
                    GameMatcher.CleaveArmamentRadius
                )
            );

            _enemies = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Enemy,
                    GameMatcher.WorldPosition));
        }

        public void Execute()
        {
            foreach (var armament in _armaments)
            {
                foreach (var enemy in _enemies.GetEntities(_buffer))
                {
                    if (enemy == null)
                        continue;

                    float distance = Vector3.Distance(armament.WorldPosition, enemy.WorldPosition);

                    if (distance <= armament.CleaveArmamentRadius)
                    {
                        armament.isReadyToApplyEffect = true;
                        armament.TargetBuffer.Add(enemy.Id);
                    }
                }

                // armament.isProcessed = true;
            }
        }
    }
}
}