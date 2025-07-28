using Entitas;
using Game.PlayerAbility.PlayerAbilityFactory;

namespace Game.PlayerAbility.SwapAbility.Systems
{
    public class DeactivateSwapAbilitySystem : IExecuteSystem
    {
        private readonly IPlayerAbilityFactory _playerAbilityFactory;
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _swapRequests;

        public DeactivateSwapAbilitySystem(GameContext game, IPlayerAbilityFactory playerAbilityFactory)
        {
            _playerAbilityFactory = playerAbilityFactory;

            _players = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Human,
                        GameMatcher.Player,
                        GameMatcher.Id));

            _swapRequests = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.SwapSelectionActive
                ));
        }

        public void Execute()
        {
            foreach (GameEntity player in _players)
            {
                if (player.GameLoopStateEnum == GameLoopStateEnum.PlaceSpirit)
                {
                    foreach (GameEntity swapRequest in _swapRequests)
                    {
                        swapRequest.isDestructed = true;
                        _playerAbilityFactory.CreateDeactivateSwapRequest(player.Id);
                    }
                }
            }
        }
    }
}