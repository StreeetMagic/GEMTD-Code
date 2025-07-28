using Entitas;
using Game.Battle.Configs;
using Services.StaticData;
using UnityEngine;

namespace Game.Battle.AuraAbility.Systems
{
    public class AuraTargetSelectionSystem : IExecuteSystem
    {
        private readonly GameContext _context;
        private readonly IStaticDataService _staticDataService;
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _targets;

        public AuraTargetSelectionSystem(GameContext context, IStaticDataService staticDataService)
        {
            _context = context;
            _staticDataService = staticDataService;

            _abilities = context.GetGroup(
                GameMatcher.AllOf
                (
                    GameMatcher.AuraAbility,
                    GameMatcher.TargetBuffer,
                    GameMatcher.ProducerId
                ));

            _targets = context.GetGroup(
                GameMatcher.AllOf
                (
                    GameMatcher.TowerEnum,
                    GameMatcher.PlayerId,
                    GameMatcher.WorldPosition
                ));
        }

        public void Execute()
        {
            foreach (var ability in _abilities)
            {
                var entityWithId = _context.GetEntityWithId(ability.ProducerId);

                if (entityWithId == null)
                    return;

                var towerConfig = _staticDataService.GetTowerConfig(entityWithId.TowerEnum);

                var towerConfigAbility = towerConfig.Abilities[0];

                var auraSetup = towerConfigAbility.AuraSetup;

                for (int index = ability.TargetBuffer.Count - 1; index >= 0; index--)
                {
                    int id = ability.TargetBuffer[index];

                    if (_context.GetEntityWithId(id) == null)
                        ability.TargetBuffer.Remove(id);
                }

                foreach (var target in _targets)
                {
                    if (target.PlayerId != _context.GetEntityWithId(ability.ProducerId).PlayerId)
                        continue;

                    AddToTargets(target, auraSetup, ability);
                }
            }
        }

        private void AddToTargets(GameEntity target, AuraSetup auraSetup, GameEntity ability)
        {
            var towerPosition = _context.GetEntityWithId(ability.ProducerId).WorldPosition;

            if (Vector3.Distance(target.WorldPosition, towerPosition) < auraSetup.Setup.Radius)
            {
                if (!ability.TargetBuffer.Contains(target.Id))
                    ability.TargetBuffer.Add(target.Id);
            }
            else
            {
                if (ability.TargetBuffer.Contains(target.Id))
                    ability.TargetBuffer.Remove(target.Id);
            }
        }
    }
}