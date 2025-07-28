using Entitas;
using Game.Battle.Extensions;
using Game.Extensions;

namespace Game.Battle
{
    public class ApplyAttackSpeedFromStatsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statOwners;

        public ApplyAttackSpeedFromStatsSystem(
            GameContext game)
        {
            _statOwners = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.BaseStats,
                    GameMatcher.StatModifiers,
                    GameMatcher.AttackSpeedStat)
            );
        }

        public void Execute()
        {
            foreach (GameEntity statOwner in _statOwners)
            {
                var projectConfigAttackSpeedStatDotaModifier =
                    statOwner
                        .AttackSpeed()
                        .ZeroIfNegative();

                statOwner.ReplaceAttackSpeedStat(projectConfigAttackSpeedStatDotaModifier);
            }
        }
    }
}