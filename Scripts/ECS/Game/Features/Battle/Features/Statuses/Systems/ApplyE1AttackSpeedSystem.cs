using System.Collections.Generic;
using Entitas;
using Game.Entity;

namespace Game.Battle
{
    public class ApplyE1AttackSpeedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statuses;
        private readonly List<GameEntity> _buffer = new(32);

        public ApplyE1AttackSpeedSystem(GameContext game)
        {
            _statuses = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.Status,
                    GameMatcher.EAura1Status,
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
                    .AddStatChange(StatEnum.AttackSpeed)
                    .AddTargetId(status.TargetId)
                    .AddProducerId(status.ProducerId)
                    .AddEffectValue(status.EffectValue)
                    .AddApplierStatusLink(status.Id);

                status.isAffected = true;
            }
        }
    }

    public class ApplyE2AttackSpeedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statuses;
        private readonly List<GameEntity> _buffer = new(32);

        public ApplyE2AttackSpeedSystem(GameContext game)
        {
            _statuses = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.Status,
                    GameMatcher.EAura2Status,
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
                    .AddStatChange(StatEnum.AttackSpeed)
                    .AddTargetId(status.TargetId)
                    .AddProducerId(status.ProducerId)
                    .AddEffectValue(status.EffectValue)
                    .AddApplierStatusLink(status.Id);

                status.isAffected = true;
            }
        }
    }

    public class ApplyE3AttackSpeedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statuses;
        private readonly List<GameEntity> _buffer = new(32);

        public ApplyE3AttackSpeedSystem(GameContext game)
        {
            _statuses = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.Status,
                    GameMatcher.EAura3Status,
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
                    .AddStatChange(StatEnum.AttackSpeed)
                    .AddTargetId(status.TargetId)
                    .AddProducerId(status.ProducerId)
                    .AddEffectValue(status.EffectValue)
                    .AddApplierStatusLink(status.Id);

                status.isAffected = true;
            }
        }
    }

    public class ApplyE4AttackSpeedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statuses;
        private readonly List<GameEntity> _buffer = new(32);

        public ApplyE4AttackSpeedSystem(GameContext game)
        {
            _statuses = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.Status,
                    GameMatcher.EAura4Status,
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
                    .AddStatChange(StatEnum.AttackSpeed)
                    .AddTargetId(status.TargetId)
                    .AddProducerId(status.ProducerId)
                    .AddEffectValue(status.EffectValue)
                    .AddApplierStatusLink(status.Id);

                status.isAffected = true;
            }
        }
    }

    public class ApplyE5AttackSpeedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statuses;
        private readonly List<GameEntity> _buffer = new(32);

        public ApplyE5AttackSpeedSystem(GameContext game)
        {
            _statuses = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.Status,
                    GameMatcher.EAura5Status,
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
                    .AddStatChange(StatEnum.AttackSpeed)
                    .AddTargetId(status.TargetId)
                    .AddProducerId(status.ProducerId)
                    .AddEffectValue(status.EffectValue)
                    .AddApplierStatusLink(status.Id);

                status.isAffected = true;
            }
        }
    }

    public class ApplyE6AttackSpeedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statuses;
        private readonly List<GameEntity> _buffer = new(32);

        public ApplyE6AttackSpeedSystem(GameContext game)
        {
            _statuses = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.Status,
                    GameMatcher.EAura6Status,
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
                    .AddStatChange(StatEnum.AttackSpeed)
                    .AddTargetId(status.TargetId)
                    .AddProducerId(status.ProducerId)
                    .AddEffectValue(status.EffectValue)
                    .AddApplierStatusLink(status.Id);

                status.isAffected = true;
            }
        }
    }
}