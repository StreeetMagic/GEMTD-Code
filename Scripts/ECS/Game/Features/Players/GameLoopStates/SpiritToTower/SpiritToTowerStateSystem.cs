using System.Collections.Generic;
using Entitas;
using Services.StaticData;

namespace Game.SpiritToTower
{
    public class SpiritToTowerStateSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _spirits;
        private readonly List<GameEntity> _buffer = new(8);
        private readonly GameEntityFactories _factories;
        private readonly IGroup<GameEntity> _players;
        private readonly IStaticDataService _config;
        private readonly GameContext _game;

        public SpiritToTowerStateSystem(
            GameContext game,
            GameEntityFactories factories,
            IStaticDataService config
        )
        {
            _game = game;
            _factories = factories;
            _config = config;

            _spirits = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.TowerSpirit
                     //   GameMatcher.Abilities
                    )
                    .NoneOf(
                        GameMatcher.Destructed
                    )
            );

            _players = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.GameLoopStateEnum
                ));
        }

        public void Execute()
        {
            foreach (GameEntity player in _players)
            {
                if (player.gameLoopStateEnum.Value != GameLoopStateEnum.SpiritToTower)
                    continue;

                if (player.hasTimer == false)
                    player.AddTimer(_config.ProjectConfig.SpiritToTowerTime);

                if (player.Timer > 0f)
                    return;

                foreach (GameEntity spirit in _spirits.GetEntities(_buffer))
                {
                    if (spirit.PlayerId != player.Id)
                        continue;

                    if (spirit.isChosen)
                    {
                        spirit.isChosen = false;
                        spirit.isTowerSpirit = false;
                        continue;
                    }

                    var worldPosX = (int)spirit.WorldPosition.x;
                    var worldPosY = (int)spirit.WorldPosition.z;

                    _factories.CreateWall(worldPosX, worldPosY, spirit.MazePosition.x, spirit.MazePosition.y,
                        player.Id);

                   DestroySpiritAbilities(spirit);
                   DestroyBasicAttackAbility(spirit);
                    
                    spirit.isDestructed = true;
                }

                player.ReplaceGameLoopStateEnum(GameLoopStateEnum.KillEnemy);
                player.RemoveTimer();
            }
        }

        private void DestroySpiritAbilities(GameEntity spirit)
        {
            foreach (var ability in spirit.Abilities)
            {
                _game.GetEntityWithId(ability).isDestructed = true;
            }
        }        
        
        private void DestroyBasicAttackAbility(GameEntity spirit)
        {
             _game.GetEntityWithId(spirit.BasicAbilityId).isDestructed = true;
        }
    }
}