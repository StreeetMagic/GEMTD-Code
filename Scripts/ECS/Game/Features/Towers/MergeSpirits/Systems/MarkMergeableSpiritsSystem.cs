using System.Collections.Generic;
using Entitas;
using Services.StaticData;

namespace Game.Towers.MergeSpirits.Systems
{
    public class MarkMergeableSpiritsSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IStaticDataService _staticData;

        private readonly IGroup<GameEntity> _spirits;
        private readonly List<GameEntity> _buffer = new(20);

        public MarkMergeableSpiritsSystem(
            GameContext game,
            IStaticDataService staticData)
        {
            _game = game;
            _staticData = staticData;

            _spirits = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.TowerSpirit,
                GameMatcher.PlayerId,
                GameMatcher.CollectingMergeVariants
            ));
        }

        public void Execute()
        {
            foreach (var spirit in _spirits.GetEntities(_buffer))
            {
                foreach (var config in _staticData.GetTowerConfigs())
                {
                    foreach (var towerEnum in config.Recipe)
                    {
                        if (towerEnum != spirit.TowerEnum)
                            continue;

                        if (!spirit.MergeVariants.Contains(config.TowerEnum))
                            spirit.MergeVariants.Add(config.TowerEnum);
                    }
                }

                spirit.isCollectingMergeVariants = false;
                spirit.isFilteringMergeVariants = true;
            }
        }
    }
}