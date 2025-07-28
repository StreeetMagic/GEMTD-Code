using Entitas;
using Game.Battle.Extensions;
using Game.Extensions;

namespace Game.Battle
{
    public class ApplyArmorFromStatsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statOwners;

        public ApplyArmorFromStatsSystem(GameContext game)
        {
            _statOwners = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.BaseStats, 
                    GameMatcher.StatModifiers, 
                    GameMatcher.Armor)
            );
        }

        public void Execute()
        {
            foreach (GameEntity statOwner in _statOwners)
            {
                statOwner.ReplaceArmor(statOwner.Armor().ZeroIfNegative());
            }
        }
    }
}