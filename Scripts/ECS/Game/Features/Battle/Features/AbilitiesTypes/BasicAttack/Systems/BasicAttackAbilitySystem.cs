using System.Collections.Generic;
using Entitas;
using Game.Battle.SplitShot.Data;
using Game.Towers;
using Services.StaticData;
using UnityEngine;

namespace Game.Battle.BasicAttack.Systems
{
    public class BasicAttackAbilitySystem : IExecuteSystem
    {
        private readonly GameContext _game;

        private readonly IArmamentFactory _armamentFactory;
        private readonly IStaticDataService _staticDataService;

        private readonly IGroup<GameEntity> _towers;
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _enemies;

        private readonly List<GameEntity> _buffer = new(4);

        public BasicAttackAbilitySystem(
            GameContext game,
            IArmamentFactory armamentFactory,
            IStaticDataService staticDataService
        )
        {
            _game = game;
            _armamentFactory = armamentFactory;
            _staticDataService = staticDataService;

            _abilities = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.BasicAttackAbility,
                    GameMatcher.CooldownUp,
                    GameMatcher.ProducerId
                ));

            _towers = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.TowerEnum,
                    GameMatcher.WorldPosition,
                    GameMatcher.PlayerId,
                    GameMatcher.AttackRange
                ));

            _enemies = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Enemy,
                    GameMatcher.WorldPosition,
                    GameMatcher.PlayerId
                ));
        }

        public void Execute()
        {
            foreach (GameEntity ability in _abilities.GetEntities(_buffer))
            {
                GameEntity tower = _game.GetEntityWithId(ability.ProducerId);

                if (tower == null)
                    continue;

                if (!tower.hasTargetId)
                    continue;

                if (tower.isTowerSpirit)
                    continue;

                TowerConfig config = _staticDataService.GetTowerConfig(tower.TowerEnum);
                Vector3 at = tower.ShootingPointWorldPosition.position;

                _armamentFactory
                    .CreateTowerBasicAttackProjectile(at, config.BasicAttackSetup, config.Abilities, tower.Rotation,
                        tower.Id)
                    .AddProducerId(tower.Id)
                    .AddTargetId(tower.TargetId)
                    .AddDirection(default)
                    ;

                if (tower.hasProjectilesCountStat && tower.ProjectilesCountStat > 1)
                    SplitShot(ability, tower);

                float cooldown = config.BasicAttackSetup.Cooldown;

                if (tower.hasAttackSpeedStat && tower.AttackSpeedStat > 0f)
                    cooldown /= tower.AttackSpeedStat * _staticDataService.ProjectConfig.AttackSpeedStatDotaModifier;

                ability.PutOnCooldown(cooldown);
            }
        }

        private void SplitShot(GameEntity ability, GameEntity tower)
        {
            SelectSplitShotTargets();

            if (ability.ProducerId != tower.Id)
                return;

            if (!tower.hasTargetId)
                return;

            if (tower.isTowerSpirit)
                return;

            TowerConfig config = _staticDataService.GetTowerConfig(tower.TowerEnum);

            Shoot(tower, config);

            foreach (TargetDistanceData? targetData in ability.SplitShotTargets)
                Shoot(tower, config, targetData);
        }

        private void Shoot(GameEntity tower, TowerConfig config, TargetDistanceData? targetData = null)
        {
            if (targetData != null)
                _armamentFactory
                    .CreateSplitshot(tower.WorldPosition, config.BasicAttackSetup, tower.Rotation, tower.Id)
                    .AddProducerId(tower.Id)
                    .AddTargetId(targetData.Value.TargetId)
                    .AddDirection(default)
                    ;
        }

        private void SelectSplitShotTargets()
        {
            foreach (GameEntity tower in _towers)
            foreach (GameEntity ability in _abilities)
            {
                if (ability.ProducerId != tower.Id)
                    continue;

                if (tower.PlayerId != ability.PlayerId)
                    continue;

                for (int i = 0; i < ability.SplitShotTargets.Length; i++)
                    ability.SplitShotTargets[i] = null;

                foreach (GameEntity enemy in _enemies)
                {
                    if (enemy.PlayerId != tower.PlayerId)
                        continue;

                    float dist = Vector3.Distance(tower.WorldPosition, enemy.WorldPosition);

                    if (dist > tower.AttackRange)
                        continue;

                    if (!tower.hasTargetId)
                        continue;

                    if (enemy.Id == tower.TargetId)
                        continue;

                    bool alreadySelected = false;

                    for (int j = 0; j < ability.SplitShotTargets.Length; j++)
                    {
                        TargetDistanceData? t = ability.SplitShotTargets[j];

                        if (t != null && t.Value.TargetId == enemy.Id)
                        {
                            alreadySelected = true;
                            break;
                        }
                    }

                    if (alreadySelected)
                        continue;

                    int freeIndex = -1;

                    for (int j = 0; j < ability.SplitShotTargets.Length; j++)
                    {
                        if (ability.SplitShotTargets[j] == null)
                        {
                            freeIndex = j;
                            break;
                        }
                    }

                    if (freeIndex != -1)
                    {
                        ability.SplitShotTargets[freeIndex] = new TargetDistanceData
                        {
                            TargetId = enemy.Id,
                            Distance = dist
                        };
                        continue;
                    }

                    int farIndex = 0;

                    float farDistance = ability.SplitShotTargets[0]!.Value.Distance;

                    for (int j = 1; j < ability.SplitShotTargets.Length; j++)
                    {
                        float distance = ability.SplitShotTargets[j]!.Value.Distance;

                        if (distance > farDistance)
                        {
                            farDistance = distance;
                            farIndex = j;
                        }
                    }

                    if (dist < farDistance)
                    {
                        ability.SplitShotTargets[farIndex] = new TargetDistanceData
                        {
                            TargetId = enemy.Id,
                            Distance = dist
                        };
                    }
                }
            }
        }
    }
}