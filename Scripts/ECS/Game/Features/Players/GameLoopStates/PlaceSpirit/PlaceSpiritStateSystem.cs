using System.Collections.Generic;
using Entitas;
using Game.Towers;
using Services.AudioServices;
using Services.AudioServices.Sounds;
using Services.ProjectData;
using Services.StaticData;
using Services.TowerRandomers;
using UnityEngine;

namespace Game.PlaceSpirit
{
    public class PlaceSpiritStateSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly SpiritFactory _spiritFactory;
        private readonly TowerRandomer _towerRandomer;
        private readonly IStaticDataService _config;
        private readonly IProjectDataService _projectDataService;
        private readonly AudioService _audioService;

        private readonly IGroup<GameEntity> _walls;
        private readonly IGroup<GameEntity> _players;
        private readonly List<GameEntity> _buffer = new(1);

        public PlaceSpiritStateSystem(
            GameContext game,
            IStaticDataService config,
            SpiritFactory spiritFactory,
            TowerRandomer towerRandomer, 
            IProjectDataService projectDataService,
            AudioService audioService)
        {
            _game = game;
            _config = config;
            _spiritFactory = spiritFactory;
            _towerRandomer = towerRandomer;
            _projectDataService = projectDataService;
            _audioService = audioService;

            _players = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.SpiritPlaced,
                    GameMatcher.Player,
                    GameMatcher.Round,
                    GameMatcher.GameLoopStateEnum
                   // GameMatcher.Human
                )
            );

            _walls = game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Wall
                    )
                    .NoneOf(
                        GameMatcher.Destructed
                    )
            );
        }

        public void Execute()
        {
            foreach (GameEntity player in _players)
            {
                if (player.hasTimer == false)
                    player.AddTimer(_config.ProjectConfig.SpiritPlacementTime);

                if (player.Timer > 0f)
                    continue;

                if (player.gameLoopStateEnum.Value != GameLoopStateEnum.PlaceSpirit)
                    continue;

                if (player.spiritPlaced.Value < _config.ProjectConfig.TowersPerRound * player.Round)
                {
                    TowerEnum towerEnum = _towerRandomer.GetTowerEnum(player.Level);

                    Vector2Int mazePosition = MazePosition(player);

                    int worldPositionX = (int)_projectDataService.StartPositions[player.Index].x + mazePosition.x;
                    int worldPositionZ = (int)_projectDataService.StartPositions[player.Index].y + mazePosition.y;

                    _spiritFactory.CreateSpirit(
                        worldPositionX,
                        worldPositionZ,
                        mazePosition.x,
                        mazePosition.y,
                        towerEnum,
                        _game.gameMainEntity.Round,
                        player.Id
                    );

                    player.ReplaceSpiritPlaced(player.spiritPlaced.Value + 1);

                    player.ReplaceGameLoopStateEnum(GameLoopStateEnum.PlayerAbility);
                }
                else
                {
                    player.ReplaceGameLoopStateEnum(GameLoopStateEnum.ChooseSpirit);
                }

                player.RemoveTimer();
            }
        }

        private Vector2Int MazePosition(GameEntity player)
        {
            Vector2Int mazePosition;
            Vector2Int[] spirits = _projectDataService.CurrentMazeData.TowerOrder;

            if (player.spiritPlaced.Value < spirits.Length)
            {
                mazePosition = spirits[player.spiritPlaced.Value];
            }
            else
            {
                float closestDistance = float.MaxValue;
                GameEntity wall = null;

                foreach (GameEntity wallEntity in _walls.GetEntities(_buffer))
                {
                    if (wallEntity.isDestructed)
                        continue;

                    if (wallEntity.PlayerId != player.Id)
                        continue;

                    float distance = wallEntity.DistanceToCenter;

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        wall = wallEntity;
                    }
                }

                if (wall == null)
                    throw new System.Exception("No wall");

                mazePosition = new Vector2Int(wall.MazePosition.x, wall.MazePosition.y);

                wall.isDestructed = true;
            }

            return mazePosition;
        }
    }
}