using System;
using Entitas;

namespace Game.PlayerAbility.SwapAbility.Systems
{
    public class SwapSelectionAbilitySystem : IExecuteSystem
    {
        private readonly GameContext _game;

        private readonly IGroup<GameEntity> _swapAbilities;
        private readonly IGroup<GameEntity> _clicks;
        private readonly IGroup<GameEntity> _highlighted;
        private readonly IGroup<GameEntity> _humans;

        public SwapSelectionAbilitySystem(GameContext game)
        {
            _game = game;

            _swapAbilities = _game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.SwapSelectionActive,
                    GameMatcher.SwapRequest,
                    GameMatcher.PlayerId
                ));

            _clicks = _game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.LeftMouseButtonClick
                ));

            _highlighted = _game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Highlighted,
                    GameMatcher.Id
                ));

            _humans = _game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.Human
                ));
        }

        public void Execute()
        {
            if (_highlighted.count > 1)
                throw new InvalidOperationException("Only one tower can be highlighted at a time!");
            
            foreach (GameEntity click in _clicks)
            foreach (GameEntity player in _humans)
            foreach (var swap in _swapAbilities)
            foreach (var highlight in _highlighted)
            {
                if (!swap.hasSwapFirstTowerSelected)
                {
                    var entity = _game.GetEntityWithId(highlight.Id);

                    if (player.Id != entity.PlayerId) 
                        continue;

                    swap.ReplaceSwapFirstTowerSelected(highlight.Id);
                    entity.isSelected = true;
                }
                else
                {
                    var firstSwapableObject = _game.GetEntityWithId(swap.SwapFirstTowerSelected);
                    
                    var entity = _game.GetEntityWithId(highlight.Id);

                    if (firstSwapableObject.Id == entity.Id)
                        continue;
                    
                    if (player.Id != entity.PlayerId) 
                        continue;

                    entity.isSelected = true;
                    
                    swap.ReplaceSwapSecondTowerSelected(highlight.Id);
                }
            }
        }
    }
}