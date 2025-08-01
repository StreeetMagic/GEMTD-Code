﻿using System.Collections.Generic;
using Entitas;
using Game.Entity;

namespace Game.Battle
{
    public class ApplyAdditionalProjectilesSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statuses;
        private readonly List<GameEntity> _buffer = new(32);

        public ApplyAdditionalProjectilesSystem(GameContext game)
        {
            _statuses = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.Status,
                    GameMatcher.AdditionalProjectilesStatus,
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
                    .AddStatChange(StatEnum.BasicAttackAdditionalProjectiles)
                    .AddTargetId(status.TargetId)
                    .AddProducerId(status.ProducerId)
                    .AddEffectValue(status.EffectValue)
                    .AddApplierStatusLink(status.Id);

                status.isAffected = true;
            }
        }
    }
}