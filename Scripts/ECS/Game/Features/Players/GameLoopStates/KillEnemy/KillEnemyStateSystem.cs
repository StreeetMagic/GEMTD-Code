using Entitas;

namespace Game.KillEnemy
{
    public class KillEnemyStateSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _roundCompletes;

        public KillEnemyStateSystem(
            GameContext game
        )
        {
            _players = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.GameLoopStateEnum
                ));

            _roundCompletes = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.RoundComplete,
                    GameMatcher.Round,
                    GameMatcher.EnemiesKilled,
                    GameMatcher.EnemiesPerRound
                )
            );
        }

        public void Execute()
        {
            foreach (GameEntity roundComplete in _roundCompletes)
            foreach (GameEntity player in _players)
            {
                if (player.gameLoopStateEnum.Value != GameLoopStateEnum.KillEnemy)
                    continue;
                
                if (player.Id != roundComplete.PlayerId)
                    continue;

                if (roundComplete.EnemiesKilled < roundComplete.EnemiesPerRound)
                    continue;
                
                roundComplete.isDestructed = true;

                player.ReplaceRound(player.Round + 1);
                player.ReplaceGameLoopStateEnum(GameLoopStateEnum.PlayerAbility);
            }
        }
    }
}