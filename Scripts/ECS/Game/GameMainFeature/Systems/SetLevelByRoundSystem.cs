using Entitas;
using Services.StaticData;

namespace Game.GameMainFeature
{
    public class SetLevelByRoundSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;

        public SetLevelByRoundSystem(
            GameContext gameContext
        )
        {
            _players = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Round,
                    GameMatcher.Player,
                    GameMatcher.Level
                ));
        }

        public void Execute()
        {
            foreach (GameEntity player in _players)
            {
                int level = 1 + player.Round / 3;
                
                if (level > 5)
                    level = 5;

                player.ReplaceLevel(level);
            }
        }
    }
}