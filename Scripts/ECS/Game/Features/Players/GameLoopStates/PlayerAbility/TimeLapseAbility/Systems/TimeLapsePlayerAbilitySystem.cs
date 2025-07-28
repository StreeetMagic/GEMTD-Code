using Entitas;
using Game.Battle;
using Game.Extensions;
using Services.StaticData;

namespace Game.PlayerAbility.TimeLapseAbility.Systems
{
    public class TimeLapsePlayerAbilitySystem : IExecuteSystem
    {
        private readonly IStaticDataService _staticDataService;
        
        private readonly IGroup<GameEntity> _requests;
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _humans;
        private readonly IGroup<GameEntity> _towers;
        private readonly IGroup<GameEntity> _walls;

        public TimeLapsePlayerAbilitySystem(GameContext game, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _abilities = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.PlayerTimeLapseAbility));
            
            _requests = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.TimeLapseRequest));
            
            _humans = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Human,
                    GameMatcher.Player,
                    GameMatcher.Id));

            _towers = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.TowerSpirit
                ));
        }

        public void Execute()
        {
            foreach (GameEntity human in _humans)
            foreach (GameEntity ability in _abilities)
            foreach (GameEntity dummy in _requests)
            {
                foreach (var tower in _towers)
                {
                    tower.isDestructed = true;
                }
                
                human.ReplaceSpiritPlaced(human.spiritPlaced.Value - _staticDataService.ProjectConfig.TowersPerRound);
                human.ReplaceGameLoopStateEnum(GameLoopStateEnum.PlaceSpirit);
                human.LoseGold(_staticDataService.GetPlayerAbilitySetup(AbilityEnum.TimeLapse).Cost);
                ability.PutOnCooldown();
            }
        }
    }
}