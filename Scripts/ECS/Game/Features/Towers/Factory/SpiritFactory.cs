using System;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Configs;
using Game.Battle.Factory;
using Game.Entity;
using Game.Extensions;
using Services.AssetProviders;
using Services.Identifiers;
using Services.StaticData;
using UnityEngine;

namespace Game.Towers
{
    public class SpiritFactory
    {
        private readonly IIdentifierService _identifiers;
        private readonly IAssetProvider _assets;
        private readonly AbilityFactory _abilityFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly VisualEffectFactory _visualEffectFactory;

        public SpiritFactory(
            IIdentifierService identifiers,
            IAssetProvider assets,
            AbilityFactory abilityFactory,
            IStaticDataService staticDataService, 
            VisualEffectFactory visualEffectFactory
            )
        {
            _identifiers = identifiers;
            _assets = assets;
            _abilityFactory = abilityFactory;
            _staticDataService = staticDataService;
            _visualEffectFactory = visualEffectFactory;
        }

        public GameEntity CreateSpirit(
            int worldPosX,
            int worldPosZ,
            int mazePosX,
            int mazePosY,
            TowerEnum towerEnum,
            int gameLoopWave,
            int playerId
        )
        {
            TowerConfig config = _staticDataService.GetTowerConfig(towerEnum);

            Dictionary<StatEnum, float> baseStats = InitStats.EmptyStatDictionary()
                    .With(x => x[StatEnum.AttackCooldown] = 0.5f)
                    .With(x => x[StatEnum.AttackRange] = 5f)
                    .With(x => x[StatEnum.AttackTimer] = 4f)
                    .With(x => x[StatEnum.BasicAttackAdditionalProjectiles] = 1f)
                    .With(x => x[StatEnum.AttackSpeed] = config.BasicAttackSetup.AttackSpeed)
                ;

            int id = _identifiers.Next();

            GameEntity basicAttackAbility =
                _abilityFactory.CreateBasicTowerAbility(id, config.BasicAttackSetup, playerId);

            List<AbilitySetup> abilitySetups = config.Abilities;

            string path = towerEnum.ToString();
            path = path.Substring(0, path.Length - 1);

            var position = new Vector3(worldPosX, 0, worldPosZ);
            _visualEffectFactory.CreatePlaceSpiritVisualEffect(position, towerEnum);
            
            GameEntity spirit = CreateGameEntity
                    .Empty()
                    .AddId(id)
                    .AddPrefab(_assets.LoadAsset(path).GetComponent<GameEntityView>())
                    .AddStatModifiers(InitStats.EmptyStatDictionary())
                    .AddBaseStats(baseStats)
                    .AddWorldPosition(position)
                    .AddMazePosition(new Vector2Int(mazePosX, mazePosY))
                    .AddTowerEnum(towerEnum)
                    .AddRound(gameLoopWave)
                    .AddAttackCooldown(baseStats[StatEnum.AttackCooldown])
                    .AddAttackRange(baseStats[StatEnum.AttackRange])
                    .AddAttackTimer(baseStats[StatEnum.AttackTimer])
                    .AddAttackSpeedStat(baseStats[StatEnum.AttackSpeed])
                    .AddRotation(Quaternion.Euler(new Vector3(0, 180, 0)))
                    .AddPlayerId(playerId)
                    .AddBasicAbilityId(basicAttackAbility.Id)
                    .AddAbilities(new List<int>())
                    .AddProjectilesCountStat(0)
                    .AddMergeVariants(new List<TowerEnum>())
                    .AddLevel(_staticDataService.ProjectConfig.TowerLevels[towerEnum])
                    .With(x => x.isCanRaycast = true)
                    .With(x => x.isTowerSpirit = true)
                    .With(x => x.isTower = true)
                    .With(x => x.isCollectingMergeVariants = true)
                    .With(x => x.isSwapable = true)
                ;

            foreach (AbilitySetup abilitySetup in abilitySetups)
            {
                switch (abilitySetup.AbilityEnum)
                {
                    case AbilityEnum.Slow:
                        spirit.Abilities.Add(_abilityFactory.CreateSlowAbility(id, abilitySetup, playerId).Id);
                        break;

                    case AbilityEnum.Pierce:
                        spirit.Abilities.Add(_abilityFactory.CreatePierceAbility(id, abilitySetup, playerId).Id);
                        break;

                    case AbilityEnum.Poison:
                        spirit.Abilities.Add(_abilityFactory.CreatePoisonAbility(id, abilitySetup, playerId).Id);
                        break;

                    case AbilityEnum.Speed:
                        spirit.Abilities.Add(_abilityFactory.CreateSpeedAbility(id, abilitySetup, playerId).Id);
                        break;

                    case AbilityEnum.Aura:
                        spirit.Abilities.Add(_abilityFactory.CreateAuraAbility(id, abilitySetup, playerId).Id);
                        break;

                    case AbilityEnum.Cleave:
                        spirit.Abilities.Add(_abilityFactory.CreateCleaveAbility(id, abilitySetup, playerId).Id);
                        break;

                    case AbilityEnum.Split:

                        spirit.Abilities.Add(_abilityFactory.CreateSplitAbility(id, abilitySetup, playerId).Id);
                        break;

                    case AbilityEnum.Unknown:
                    case AbilityEnum.BasicAttack:
                    default:
                        throw new Exception($"Ability with type id {abilitySetup.AbilityEnum} does not exist");
                }
            }

            return spirit;
        }
    }
}