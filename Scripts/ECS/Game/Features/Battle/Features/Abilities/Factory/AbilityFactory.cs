using System.Collections.Generic;
using System.Linq;
using Game.Battle.Configs;
using Game.Battle.SplitShot.Data;
using Game.Entity;
using Game.Extensions;
using Services.Identifiers;

namespace Game.Battle.Factory
{
    public class AbilityFactory
    {
        private readonly IIdentifierService _identifiers;

        public AbilityFactory(IIdentifierService identifiers)
        {
            _identifiers = identifiers;
        }

        public GameEntity CreateBasicTowerAbility(int towerId, AbilitySetup setup, int playerId)
        {
            var ability = CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddAbilityEnum(setup.AbilityEnum)
                    .AddCooldown(setup.Cooldown)
                    .AddProducerId(towerId)
                    .AddTargetId(towerId)
                    .AddPlayerId(playerId)
                    .AddTargetBuffer(new List<int> { towerId })
                    .AddStatusSetups(setup.StatusSetups.ToList())
                    .With(x => x.AddSplitShotTargets(new TargetDistanceData?[2]),
                        when: setup.AbilityEnum == AbilityEnum.BasicAttack)
                    .PutOnCooldown()
                ;

            ability.AddAbilityComponent(setup.AbilityEnum);

            return ability;
        }

        public GameEntity CreateSlowAbility(int towerId, AbilitySetup setup, int playerId)
        {
            var ability = CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddAbilityEnum(setup.AbilityEnum)
                    .AddCooldown(setup.Cooldown)
                    .AddProducerId(towerId)
                    .AddPlayerId(playerId)
                    .AddTargetId(towerId)
                    .AddStatusSetups(setup.StatusSetups.ToList())
                    .PutOnCooldown()
                ;

            ability.AddAbilityComponent(setup.AbilityEnum);

            return ability;
        }

        public GameEntity CreatePierceAbility(int towerId, AbilitySetup setup, int playerId)
        {
            var ability = CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddAbilityEnum(setup.AbilityEnum)
                    .AddCooldown(setup.Cooldown)
                    .AddProducerId(towerId)
                    .AddPlayerId(playerId)
                    .AddTargetId(towerId)
                    .AddStatusSetups(setup.StatusSetups.ToList())
                    .PutOnCooldown()
                ;

            ability.AddAbilityComponent(setup.AbilityEnum);

            return ability;
        }

        public GameEntity CreateCleaveAbility(int towerId, AbilitySetup setup, int playerId)
        {
            var ability = CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddAbilityEnum(setup.AbilityEnum)
                    .AddCooldown(setup.Cooldown)
                    .AddProducerId(towerId)
                    .AddPlayerId(playerId)
                    .AddTargetId(towerId)
                    .AddEffectSetups(setup.EffectSetups.ToList())
                    .AddStatusSetups(setup.StatusSetups.ToList())
                    .PutOnCooldown()
                ;

            ability.AddAbilityComponent(setup.AbilityEnum);

            return ability;
        }

        public GameEntity CreatePoisonAbility(int towerId, AbilitySetup setup, int playerId)
        {
            var ability = CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddAbilityEnum(setup.AbilityEnum)
                    .AddCooldown(setup.Cooldown)
                    .AddProducerId(towerId)
                    .AddPlayerId(playerId)
                    .AddTargetId(towerId)
                    .AddStatusSetups(setup.StatusSetups.ToList())
                    .PutOnCooldown()
                ;

            ability.AddAbilityComponent(setup.AbilityEnum);

            return ability;
        }

        public GameEntity CreateSpeedAbility(int towerId, AbilitySetup setup, int playerId)
        {
            var ability = CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddAbilityEnum(setup.AbilityEnum)
                    .AddTargetBuffer(new List<int> { towerId })
                    .AddCooldown(setup.Cooldown)
                    .AddProducerId(towerId)
                    .AddPlayerId(playerId)
                    .AddTargetId(towerId)
                    .AddStatusSetups(setup.StatusSetups.ToList())
                    .PutOnCooldown()
                ;

            ability.AddAbilityComponent(setup.AbilityEnum);

            return ability;
        }

        public GameEntity CreateAuraAbility(int towerId, AbilitySetup setup, int playerId)
        {
            var ability = CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddAbilityEnum(setup.AbilityEnum)
                    .AddTargetBuffer(new List<int>())
                    .AddCooldown(setup.Cooldown)
                    .AddProducerId(towerId)
                    .AddPlayerId(playerId)
                    .AddTargetId(towerId)
                    .AddStatusSetups(setup.StatusSetups.ToList())
                    .PutOnCooldown()
                ;

            ability.AddAbilityComponent(setup.AbilityEnum);

            return ability;
        }

        public GameEntity CreateSplitAbility(int towerId, AbilitySetup setup, int playerId)
        {
            var ability = CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddAbilityEnum(setup.AbilityEnum)
                    .AddTargetBuffer(new List<int> { towerId })
                    .AddCooldown(setup.Cooldown)
                    .AddProducerId(towerId)
                    .AddPlayerId(playerId)
                    .AddTargetId(towerId)
                    .AddStatusSetups(setup.StatusSetups.ToList())
                    .PutOnCooldown()
                ;

            ability.AddAbilityComponent(setup.AbilityEnum);

            return ability;
        }
    }
}