using System.Collections.Generic;
using Entitas;
using Game.Entity;
using Game.Extensions;

namespace Game.Enemies
{
    public class FinalizeEnemyDeathProcessingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _enemies;
        private readonly List<GameEntity> _buffer = new(128);

        public FinalizeEnemyDeathProcessingSystem(GameContext game)
        {
            _enemies = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Enemy,
                    GameMatcher.Dead,
                    GameMatcher.ProcessingDeath,
                    GameMatcher.Gold
                )
                .NoneOf(
                    GameMatcher.EnemyFragCreated
                ));
        }

        public void Execute()
        {
            foreach (GameEntity enemy in _enemies.GetEntities(_buffer))
            {
                enemy.isProcessingDeath = false;
                enemy.isEnemyFragCreated = true;

                CreateGameEntity
                    .Empty()
                    .AddRound(enemy.Round)
                    .AddPlayerId(enemy.PlayerId)
                    .AddGold(enemy.Gold)
                    .With(x => x.isEnemyFrag = true);
            }
        }
    }
}