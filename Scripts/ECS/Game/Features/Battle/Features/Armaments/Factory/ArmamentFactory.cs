using System.Collections.Generic;
using System.Linq;
using Game.Battle.Configs;
using Game.Entity;
using Game.Extensions;
using Services.Identifiers;
using UnityEngine;

namespace Game.Battle
{
    public class ArmamentFactory : IArmamentFactory
    {
        private const int TargetBufferSize = 16;

        private readonly IIdentifierService _identifiers;
        private readonly GameContext _game;
        private readonly VisualEffectFactory _visualEffectFactory;

        public ArmamentFactory(
            IIdentifierService identifiers,
            GameContext game, 
            VisualEffectFactory visualEffectFactory
            )
        {
            _identifiers = identifiers;
            _game = game;
            _visualEffectFactory = visualEffectFactory;
        }

        public GameEntity CreateTowerBasicAttackProjectile(
            Vector3 at,
            AbilitySetup abilitySetup,
            List<AbilitySetup> abilitySetups,
            Quaternion rotation,
            int towerId
        )
        {
            Dictionary<StatEnum, float> baseStats = InitStats.EmptyStatDictionary()
                .With(x => x[StatEnum.MoveSpeed] = abilitySetup.ProjectileSetup.MoveSpeed);

            var armament = CreateGameEntity
                    .Empty()
                    .AddId(_identifiers.Next())
                    .AddPrefab(abilitySetup.ViewPrefab)
                    .AddWorldPosition(at)
                    .AddEffectSetups(abilitySetup.EffectSetups)
                    .AddStatusSetups(abilitySetup.StatusSetups.ToList())
                    .AddBaseStats(baseStats)
                    .AddStatModifiers(InitStats.EmptyStatDictionary())
                    .AddMoveSpeedStat(baseStats[StatEnum.MoveSpeed])
                    .AddRadius(abilitySetup.ProjectileSetup.ContactRadius)
                    .AddTargetBuffer(new List<int>(TargetBufferSize))
                    .AddProcessedTargets(new List<int>(TargetBufferSize))
                    .AddLayerMask(CollisionLayerEnum.Enemy.AsMask())
                    .With(x => x.isArmament = true)
                    .With(x => x.AddTargetLimit(1))
                    .With(x => x.isMovementAvailable = true)
                    .With(x => x.isReadyToCollectTargets = true)
                    .With(x => x.isCollectingTargetsContinuously = true)
                    .With(x => x.StatusSetups.Add(GetStatusSetup(abilitySetups, AbilityEnum.Slow)),
                        when: _game.GetEntityWithId(towerId).isSlowProjectiles)
                    .With(x => x.StatusSetups.Add(GetStatusSetup(abilitySetups, AbilityEnum.Pierce)),
                        when: _game.GetEntityWithId(towerId).isPierceProjectiles)
                    .With(x => x.StatusSetups.Add(GetStatusSetup(abilitySetups, AbilityEnum.Poison)),
                        when: _game.GetEntityWithId(towerId).isPoisonProjectiles)
                ;

            _visualEffectFactory.CreateMuzzleFlashEffect(at, rotation, towerId);

            return armament;
        }

        public GameEntity CreateSplitshot(
            Vector3 at,
            AbilitySetup abilitySetup,
            Quaternion rotation,
            int towerId
        )
        {
            Dictionary<StatEnum, float> baseStats = InitStats.EmptyStatDictionary()
                .With(x => x[StatEnum.MoveSpeed] = abilitySetup.ProjectileSetup.MoveSpeed);

            GameEntity gameEntity = CreateGameEntity
                    .Empty()
                    .AddId(_identifiers.Next())
                    .AddPrefab(abilitySetup.ViewPrefab)
                    .AddWorldPosition(at)
                    .AddEffectSetups(abilitySetup.EffectSetups)
                    .AddStatusSetups(abilitySetup.StatusSetups)
                    .AddBaseStats(baseStats)
                    .AddStatModifiers(InitStats.EmptyStatDictionary())
                    .AddMoveSpeedStat(baseStats[StatEnum.MoveSpeed])
                    .AddRadius(abilitySetup.ProjectileSetup.ContactRadius)
                    .AddTargetBuffer(new List<int>(TargetBufferSize))
                    .AddProcessedTargets(new List<int>(TargetBufferSize))
                    .AddLayerMask(CollisionLayerEnum.Enemy.AsMask())
                    .AddSelfDestructTimer(3)
                    .With(x => x.isArmament = true)
                    .With(x => x.AddTargetLimit(1))
                    .With(x => x.isMovementAvailable = true)
                    .With(x => x.isReadyToCollectTargets = true)
                    .With(x => x.isCollectingTargetsContinuously = true)
                ;

            _visualEffectFactory.CreateMuzzleFlashEffect(at, rotation, towerId);

            return gameEntity;
        }

        public GameEntity CreateCleaveRequest(GameEntity armament)
        {
            return CreateGameEntity
                .Empty()
                .AddWorldPosition(armament.WorldPosition)
                .AddProducerId(armament.ProducerId)
                .With(x => x.isReadyToCleave = true);
        }

        public GameEntity CreateCleave(
            Vector3 at,
            AbilitySetup abilitySetup
        )
        {
            Dictionary<StatEnum, float> baseStats = InitStats.EmptyStatDictionary()
                .With(x => x[StatEnum.MoveSpeed] = abilitySetup.ProjectileSetup.MoveSpeed);

            return CreateGameEntity
                    .Empty()
                    .AddId(_identifiers.Next())
                    .AddWorldPosition(at)
                    .AddEffectSetups(abilitySetup.EffectSetups)
                    .AddBaseStats(baseStats)
                    .AddStatModifiers(InitStats.EmptyStatDictionary())
                    .AddMoveSpeedStat(baseStats[StatEnum.MoveSpeed])
                    .AddRadius(abilitySetup.ProjectileSetup.ContactRadius)
                    .AddTargetBuffer(new List<int>(TargetBufferSize))
                    .AddProcessedTargets(new List<int>(TargetBufferSize))
                    .AddLayerMask(CollisionLayerEnum.Enemy.AsMask())
                    .AddSelfDestructTimer(3f)
                    .AddCleaveArmamentRadius(2.5f)
                    .With(x => x.isArmament = true)
                    .With(x => x.isMovementAvailable = true)
                    .With(x => x.isReadyToCollectTargets = true)
                    .With(x => x.isCollectingTargetsContinuously = true)
                    .With(x => x.isCleaveArmament = true)
                ;
        }

        private static StatusSetup GetStatusSetup(List<AbilitySetup> abilitySetups, AbilityEnum abilityEnum)
        {
            StatusSetup statusSetup = null;

            foreach (var setup in abilitySetups)
            {
                if (setup.AbilityEnum != abilityEnum)
                    continue;

                statusSetup = setup.StatusSetups[0];

                break;
            }

            return statusSetup;
        }
    }
}