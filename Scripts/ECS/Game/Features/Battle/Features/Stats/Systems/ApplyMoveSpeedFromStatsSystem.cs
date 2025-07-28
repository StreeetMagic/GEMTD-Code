using Entitas;
using Game.Battle.Extensions;
using Game.Extensions;

namespace Game.Battle
{
    public class ApplyMoveSpeedFromStatsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statOwners;

        public ApplyMoveSpeedFromStatsSystem(GameContext game)
        {
            _statOwners = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.BaseStats,
                    GameMatcher.StatModifiers,
                    GameMatcher.MoveSpeedStat)
            );
        }

        public void Execute()
        {
            foreach (GameEntity statOwner in _statOwners)
            {
                statOwner.ReplaceMoveSpeedStat(statOwner.MoveSpeed().ZeroIfNegative());
            }
        }
    }
}