using Entitas;

namespace Game.Battle.SlowAbility.Systems
{
    public class SlowAbilitySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _towers;
        
        public SlowAbilitySystem(GameContext game)
        {
            _abilities = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.SlowAbility,
                    GameMatcher.ProducerId));

            _towers = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.TowerEnum));
        }

        public void Execute()
        {
            foreach (GameEntity ability in _abilities)
            foreach (GameEntity tower in _towers)
            {
                if(tower.Id != ability.ProducerId)
                    continue;
                
                if(!ability.isCooldownUp)
                    continue;
                
                tower.isSlowProjectiles = true;
                
                ability.PutOnCooldown();
            }
        }
    }
}

