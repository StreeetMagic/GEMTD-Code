using System.Collections.Generic;
using Entitas;
using Game.Entity;

namespace Game.Battle
{
    public class ApplyDecreaseArmorStatusSystem : IExecuteSystem
    {
        private readonly IStatusApplier _statusApplier;
        private readonly IGroup<GameEntity> _statuses;
        private readonly List<GameEntity> _buffer = new(32);

        public ApplyDecreaseArmorStatusSystem(GameContext game)
        {
            _statuses = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.Status,
                    GameMatcher.DecreaseArmor,
                    GameMatcher.ProducerId,
                    GameMatcher.TargetId,
                    GameMatcher.EffectValue)
                .NoneOf(GameMatcher.Affected));
        }

        public void Execute()
        {
            foreach (GameEntity status in _statuses.GetEntities(_buffer))
            {
                CreateGameEntity.Empty()
                    .AddStatChange(StatEnum.Armor)
                    .AddTargetId(status.TargetId)
                    .AddProducerId(status.ProducerId)
                    .AddEffectValue(status.EffectValue)
                    .AddApplierStatusLink(status.Id);
                
                status.isAffected = true;
            }
        }
    }
}