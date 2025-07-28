using Entitas;
using Game.Battle.Extensions;
using Game.Extensions;

namespace Game.Battle
{
    public class ApplyAdditionalProjectilesFromStatsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statOwners;

        public ApplyAdditionalProjectilesFromStatsSystem(GameContext game)
        {
            _statOwners = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.BaseStats,
                    GameMatcher.StatModifiers,
                    GameMatcher.ProjectilesCountStat)
            );
        }

        public void Execute()
        {
            foreach (GameEntity statOwner in _statOwners)
            {
                statOwner.ReplaceProjectilesCountStat(statOwner.AdditionalProjectiles().ZeroIfNegative());
            }
        }
    }
}