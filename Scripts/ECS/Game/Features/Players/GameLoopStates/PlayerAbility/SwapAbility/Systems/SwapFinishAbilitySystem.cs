using System.Collections.Generic;
using Entitas;
using Game.Battle;
using Game.Extensions;
using Services.StaticData;
using UnityEngine;

namespace Game.PlayerAbility.SwapAbility.Systems
{
    public class SwapFinishAbilitySystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly GameEntityFactories _entityFactories;
        private readonly IStaticDataService _staticDataService;

        private readonly List<GameEntity> _buffer = new(4);

        private readonly IGroup<GameEntity> _finishRequests;
        private readonly IGroup<GameEntity> _swapAbilities;

        public SwapFinishAbilitySystem(
            GameContext game,
            GameEntityFactories entityFactories,
            IStaticDataService staticDataService
        )
        {
            _game = game;
            _entityFactories = entityFactories;
            _staticDataService = staticDataService;

            _swapAbilities = _game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.PlayerSwapAbility
                ));

            _finishRequests = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.SwapFinishRequest,
                    GameMatcher.SwapFirstTowerSelected,
                    GameMatcher.SwapSecondTowerSelected
                ));
        }

        public void Execute()
        {
            foreach (GameEntity swapAbility in _swapAbilities)
            foreach (GameEntity finishRequest in _finishRequests.GetEntities(_buffer))
            {
                var firstTower = _game.GetEntityWithId(finishRequest.SwapFirstTowerSelected);
                var secondTower = _game.GetEntityWithId(finishRequest.SwapSecondTowerSelected);

                if (firstTower.Id == secondTower.Id)
                    continue;
                
                Vector2Int firstTowerMazePosition = firstTower.MazePosition;
                Vector3 firstTowerWorldPosition = firstTower.WorldPosition;

                firstTower.ReplaceMazePosition(secondTower.MazePosition);
                firstTower.ReplaceWorldPosition(secondTower.WorldPosition);

                secondTower.ReplaceMazePosition(firstTowerMazePosition);
                secondTower.ReplaceWorldPosition(firstTowerWorldPosition);

                firstTower.isSelected = false;
                secondTower.isSelected = false;

                swapAbility.PutOnCooldown();

                _entityFactories.CreateCancelSelectionRequest();
                
                GameEntity player = _game.GetEntityWithId(firstTower.PlayerId);
                player.LoseGold(_staticDataService.GetPlayerAbilitySetup(AbilityEnum.SwapTowers).Cost);
            }
        }
    }
}