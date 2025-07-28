using System.Collections.Generic;
using Entitas;
using Game.Extensions;
using Services.StaticData;
using UnityEngine;

namespace Game.Enemies
{
    public class CountKilledEnemiesSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _roundCompletes;
        private readonly IGroup<GameEntity> _frags;
        private readonly List<GameEntity> _buffer = new List<GameEntity>(1);
        private readonly GameContext _game;
        private readonly IStaticDataService _staticDataService;

        public CountKilledEnemiesSystem(
            GameContext game,
            IStaticDataService staticDataService
        )
        {
            _game = game;
            _staticDataService = staticDataService;
            _frags = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Round,
                        GameMatcher.PlayerId,
                        GameMatcher.EnemyFrag,
                        GameMatcher.Gold
                    )
            );

            _roundCompletes = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.RoundComplete,
                        GameMatcher.PlayerId,
                        GameMatcher.Round,
                        GameMatcher.EnemiesKilled
                    )
            );
        }

        public void Execute()
        {
            foreach (GameEntity frag in _frags.GetEntities(_buffer))
            foreach (GameEntity roundComplete in _roundCompletes)
            {
                if (frag.PlayerId != roundComplete.PlayerId)
                    continue;

                if (frag.Round != roundComplete.Round)
                    continue;

                var player = _game.GetEntityWithId(roundComplete.PlayerId);

                if (player.isHuman)
                {
                    player.GainGold(frag.Gold);
                }

                roundComplete.ReplaceEnemiesKilled(roundComplete.EnemiesKilled + 1);

                frag.isDestructed = true;
                frag.isEnemyFrag = false;
            }
        }
    }
}