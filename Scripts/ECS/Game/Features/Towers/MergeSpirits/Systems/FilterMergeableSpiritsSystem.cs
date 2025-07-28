using Entitas;
using Services.StaticData;
using UnityEngine.Pool;

namespace Game.Towers.MergeSpirits.Systems
{
    public class FilterMergeableSpiritsSystem : IExecuteSystem
    {
        private readonly IStaticDataService _staticData;

        private readonly IGroup<GameEntity> _spirits;
        private readonly IGroup<GameEntity> _players;

        public FilterMergeableSpiritsSystem(
            GameContext game,
            IStaticDataService staticData)
        {
            _staticData = staticData;

            _spirits = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.TowerSpirit,
                GameMatcher.PlayerId,
                GameMatcher.MergeVariants,
                GameMatcher.FilteringMergeVariants
            ));

            _players = game.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.GameLoopStateEnum));
        }

        public void Execute()
        {
            foreach (var player in _players)
            {
                if (player.gameLoopStateEnum.Value != GameLoopStateEnum.ChooseSpirit)
                    continue;

                using var _1 = ListPool<GameEntity>.Get(out var playerSpirits);

                foreach (var spirit in _spirits)
                {
                    if (spirit.PlayerId == player.Id)
                        playerSpirits.Add(spirit);
                }

                foreach (var playerSpirit1 in playerSpirits)
                {
                    for (int i = playerSpirit1.MergeVariants.Count - 1; i >= 0; i--)
                    {
                        TowerEnum target = playerSpirit1.MergeVariants[i];
                        TowerConfig config = _staticData.GetTowerConfig(target);

                        using var _2 = ListPool<TowerEnum>.Get(out var playerEnums);
                        
                        foreach (var playerSpirit2 in playerSpirits)
                            playerEnums.Add(playerSpirit2.TowerEnum);

                        bool isValid = true;

                        foreach (var required in config.Recipe)
                        {
                            if (!playerEnums.Remove(required))
                            {
                                isValid = false;
                                break;
                            }
                        }

                        if (!isValid)
                            playerSpirit1.MergeVariants.RemoveAt(i);
                    }

                    playerSpirit1.isFilteringMergeVariants = false;
                }
            }
        }
    }
}