using System.Collections.Generic;
using Entitas;
using Game.Battle;
using Zenject;

namespace Game.EntityIndices
{
    public class GameEntityIndices : IInitializable
    {
        private readonly GameContext _game;

        public const string StatusesOfType = "StatusesOfType";
        public const string StatChanges = "StatChanges";

        public GameEntityIndices(GameContext game)
        {
            _game = game;
        }

        public void Initialize()
        {
            _game.AddEntityIndex(new EntityIndex<GameEntity, StatusKey>(
                name: StatusesOfType,
                _game.GetGroup(GameMatcher.AllOf(
                    GameMatcher.StatusTypeId,
                    GameMatcher.TargetId,
                    GameMatcher.Status,
                    GameMatcher.Duration,
                    GameMatcher.TimeLeft)),
                getKey: GetTargetStatusKey,
                new StatusKeyEqualityComparer()));

            _game.AddEntityIndex(new EntityIndex<GameEntity, StatKey>(
                name: StatChanges,
                _game.GetGroup(GameMatcher.AllOf(
                    GameMatcher.StatChange,
                    GameMatcher.TargetId)),
                getKey: GetTargetStatKey,
                new StatKeyEqualityComparer()));
        }

        private StatKey GetTargetStatKey(GameEntity entity, IComponent component)
        {
            return new StatKey(
                (component as TargetIdComponent)?.Value ?? entity.TargetId,
                (component as StatChangeComponent)?.Value ?? entity.StatChange);
        }

        private StatusKey GetTargetStatusKey(GameEntity entity, IComponent component)
        {
            return new StatusKey(
                (component as TargetIdComponent)?.Value ?? entity.TargetId,
                (component as StatusTypeIdComponent)?.Value ?? entity.StatusTypeId);
        }
    }

    public static class ContextIndicesExtensions
    {
        public static HashSet<GameEntity> TargetStatusesOfType(this GameContext context, StatusEnum statusTypeId,
            int targetId)
        {
            return ((EntityIndex<GameEntity, StatusKey>)context.GetEntityIndex(GameEntityIndices.StatusesOfType))
                .GetEntities(new StatusKey(targetId, statusTypeId));
        }

        public static HashSet<GameEntity> TargetStatChanges(this GameContext context, StatEnum stat, int targetId)
        {
            return ((EntityIndex<GameEntity, StatKey>)context.GetEntityIndex(GameEntityIndices.StatChanges))
                .GetEntities(new StatKey(targetId, stat));
        }
    }
}