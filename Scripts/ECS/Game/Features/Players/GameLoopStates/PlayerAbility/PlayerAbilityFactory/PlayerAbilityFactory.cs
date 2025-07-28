using System.Collections.Generic;
using Game.Battle;
using Game.Entity;
using Game.Extensions;
using Services.Identifiers;
using Services.StaticData;

namespace Game.PlayerAbility.PlayerAbilityFactory
{
    public class PlayerAbilityFactory : IPlayerAbilityFactory
    {
        private readonly IIdentifierService _identifierService;
        private readonly IStaticDataService _staticDataService;

        public PlayerAbilityFactory(
            IIdentifierService identifierService, 
            IStaticDataService staticDataService
            )
        {
            _identifierService = identifierService;
            _staticDataService = staticDataService;
        }

        public void CreatePlayerSwapAbility(int realPlayerId)
        {
            var setup = _staticDataService.GetPlayerAbilitySetup(AbilityEnum.SwapTowers);
            
            CreateGameEntity.Empty()
                .AddId(_identifierService.Next())
                .AddProducerId(realPlayerId)
                .AddAbilityEnum(AbilityEnum.SwapTowers)
                .AddCooldown(setup.Cooldown)
                .AddSwapCostInGold(setup.Cost)
                .With(x => x.isPlayerSwapAbility = true)
                .PutOnCooldown()
                ;
        }

        public void CreateHealThroneAbility(int playerId)
        {
            var setup = _staticDataService.GetPlayerAbilitySetup(AbilityEnum.HealThrone);
            
            CreateGameEntity.Empty()
                .AddId(_identifierService.Next())
                .AddProducerId(playerId)
                .AddAbilityEnum(AbilityEnum.HealThrone)
                .AddCooldown(setup.Cooldown)
                .AddEffectSetups(new List<EffectSetup>(setup.EffectSetups))
                .AddHealThroneCostInGold(setup.Cost)
                .With(x => x.isPlayerHealThroneAbility = true)
                .PutOnCooldown()
                ;
        }
        
        public void CreateTimeLapseAbility(int playerId)
        {
            var setup = _staticDataService.GetPlayerAbilitySetup(AbilityEnum.TimeLapse);
            
            CreateGameEntity.Empty()
                .AddId(_identifierService.Next())
                .AddProducerId(playerId)
                .AddAbilityEnum(AbilityEnum.TimeLapse)
                .AddCooldown(setup.Cooldown)
                .AddTimeLapseCostInGold(setup.Cost)
                .With(x => x.isPlayerTimeLapseAbility = true)
                .PutOnCooldown()
                ;
        }

        public void CreateSwapRequest(int playerId)
        {
            CreateGameEntity.Empty()
                .AddId(_identifierService.Next())
                .AddPlayerId(playerId)
                .AddProducerId(playerId)
                .With(x => x.isSwapRequest = true)
                .With(x => x.isSwapSelectionActive = true)
                ;
        }

        public void CreateDeactivateSwapRequest(int playerId)
        {
            CreateGameEntity.Empty()
                .AddId(_identifierService.Next())
                .AddPlayerId(playerId)
                .AddProducerId(playerId)
                .With(x => x.isSwapRequest = true)
                .With(x => x.isSwapSelectionDeactivate = true)
                ;
        }

        public void CreateHealThroneRequest(int playerId)
        {
            CreateGameEntity.Empty()
                .AddId(_identifierService.Next())
                .AddPlayerId(playerId)
                .AddProducerId(playerId)
                .With(x => x.isHealThroneRequest = true)
                .With(x => x.isDestructed = true)
                ;
        }

        public void CreateTimeLapseRequest(int playerId)
        {
            CreateGameEntity.Empty()
                .AddId(_identifierService.Next())
                .AddPlayerId(playerId)
                .AddProducerId(playerId)
                .With(x => x.isTimeLapseRequest = true)
                .With(x => x.isDestructed = true)
                ;
        }
    }
}