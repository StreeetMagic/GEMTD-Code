using Entitas;
using Services.StaticData;

namespace Game.PlayerAbility.Systems
{
    public class PlayerAbilityStateSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IStaticDataService _config;

        public PlayerAbilityStateSystem(
            GameContext gameContext,
            IStaticDataService config
        )
        {
            _config = config;

            _players = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.GameLoopStateEnum
                ));
        }

        public void Execute()
        {
            foreach (GameEntity player in _players)
            {
                if (player.gameLoopStateEnum.Value != GameLoopStateEnum.PlayerAbility)
                    continue;

                if (player.hasTimer == false)
                    player.AddTimer(_config.ProjectConfig.PlayerAbilityTime);

                if (player.Timer > 0f)
                    return;

                player.ReplaceGameLoopStateEnum(GameLoopStateEnum.PlaceSpirit);

                player.RemoveTimer();
            }
        }
    }
}