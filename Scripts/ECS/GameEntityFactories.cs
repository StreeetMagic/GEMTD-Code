using System.Collections.Generic;
using Game;
using Game.Battle;
using Game.Enemies;
using Game.Entity;
using Game.Extensions;
using Game.PlayerAbility.PlayerAbilityFactory;
using Game.Towers;
using Services.AssetProviders;
using Services.Identifiers;
using Services.ProjectData;
using Services.StaticData;
using UnityEngine;

public class GameEntityFactories
{
    private readonly IIdentifierService _identifierService;
    private readonly IAssetProvider _assetProvider;
    private readonly IStaticDataService _staticDataService;
    private readonly IProjectDataService _projectDataService;
    private readonly IPlayerAbilityFactory _playerAbilityFactory;

    public GameEntityFactories(
        IIdentifierService identifierService,
        IAssetProvider assetProvider,
        IStaticDataService staticDataService,
        GameContext game,
        IProjectDataService projectDataService,
        IPlayerAbilityFactory playerAbilityFactory
    )
    {
        _identifierService = identifierService;
        _assetProvider = assetProvider;
        _staticDataService = staticDataService;
        _projectDataService = projectDataService;
        _playerAbilityFactory = playerAbilityFactory;
    }

    public void CreateSpiritSelectRequest(GameEntity entity)
    {
        CreateGameEntity.Empty()
            .AddId(_identifierService.Next())
            .AddPlayerId(entity.PlayerId)
            .AddWorldPosition(entity.WorldPosition)
            .AddMazePosition(entity.MazePosition)
            .AddTowerEnum(entity.TowerEnum)
            .With(x => x.isRequestSpiritSelect = true)
            ;
    }

    public void CreateSpiritsMergeRequest(GameEntity entity, TowerEnum targetTowerType)
    {
        CreateGameEntity.Empty()
            .AddId(_identifierService.Next())
            .AddWorldPosition(entity.WorldPosition)
            .AddTowerEnum(targetTowerType)
            .AddMazePosition(entity.MazePosition)
            .AddPlayerId(entity.PlayerId)
            .With(x => x.isMergeResult = true)
            ;
    }

    public void CreateCursor()
    {
        CreateGameEntity
            .Empty()
            .AddCursorPosition(Vector2.zero)
            .AddId(_identifierService.Next())
            .With(x => x.isCursor = true)
            ;
    }

    public void CreateStartPoint(
        int positionX,
        int positionY,
        int playerId,
        bool isHuman
    )
    {
        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPrefab(_assetProvider.LoadAsset("Portal").GetComponent<GameEntityView>())
            .AddWorldPosition(new Vector3(positionX, 0, positionY))
            .AddPlayerId(playerId)
            .With(x => x.isStartPoint = true)
            .With(x => x.isHuman = isHuman)
            ;
    }

    public void CreateFinishPoint(
        int positionX,
        int positionY,
        int playerId,
        bool isHuman
    )
    {
        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPrefab(_assetProvider.LoadAsset("Throne").GetComponent<GameEntityView>())
            .AddWorldPosition(new Vector3(positionX, 0, positionY))
            .AddPlayerId(playerId)
            .With(x => x.isFinishPoint = true)
            .With(x => x.isHuman = isHuman)
            ;
    }

    public void CreateCheckpoint(
        int positionX,
        int positionY,
        int number,
        int playerId,
        bool isHuman
    )
    {
        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPrefab(_assetProvider.LoadAsset("CheckPoint").GetComponent<GameEntityView>())
            .AddWorldPosition(new Vector3(positionX, 0, positionY))
            .AddCheckPoint(number)
            .AddPlayerId(playerId)
            .With(x => x.isCanRaycast = true)
            .With(x => x.isHuman = isHuman)
            ;
    }

    public void CreateWall(
        int worldPositionX,
        int worldPositionY,
        int mazePositionX,
        int mazePositionY,
        int playerId
    )
    {
        float length = _projectDataService.CurrentMazeData.Height;
        float width = _projectDataService.CurrentMazeData.Width;

        int centerX = (int)(length / 2 + 0.5f);
        int centerY = (int)(width / 2 + 0.5f);

        float distanceToCenter =
            Vector2.Distance(new Vector2(mazePositionX, mazePositionY), new Vector2(centerX, centerY));

        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPrefab(_assetProvider.LoadAsset("Wall").GetComponent<GameEntityView>())
            .AddWorldPosition(new Vector3(worldPositionX, 0, worldPositionY))
            .AddMazePosition(new Vector2Int(mazePositionX, mazePositionY))
            .AddPlayerId(playerId)
            .AddDistanceToCenter(distanceToCenter)
            .With(x => x.isWall = true)
            .With(x => x.isCanRaycast = true)
            .With(x => x.isSwapable = true)
            ;
    }

    public void CreateEnemy(
        Vector2Int position,
        int round,
        int playerId,
        bool isHuman
    )
    {
        EnemyConfig config = _staticDataService.GetEnemyConfig(round);
        EnemyCoefficient enemyCoefficient =
            _staticDataService.GetEnemyCoefficient(_projectDataService.CurrentGameModeType);

        Dictionary<StatEnum, float> baseStats = InitStats.EmptyStatDictionary()
                .With(x => x[StatEnum.MoveSpeed] = config.MoveSpeed)
                .With(x => x[StatEnum.MaxHeathPoints] = config.MaxHealthPoints)
                .With(x => x[StatEnum.RotationSpeed] = config.RotationSpeed)
                .With(x => x[StatEnum.Armor] = config.Armor)
            ;

        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPrefab(config.Prefab)
            .AddStatModifiers(InitStats.EmptyStatDictionary())
            .AddBaseStats(baseStats)
            .AddWorldPosition(new Vector3(position.x, 0, position.y))
            .AddRotation(default)
            .AddDirection(default)
            .AddMoveSpeedStat(baseStats[StatEnum.MoveSpeed] * enemyCoefficient.Speed)
            .AddTargetPlaceIndex(0)
            .AddPathNumber(0)
            .AddCurrentHealthPoints(baseStats[StatEnum.MaxHeathPoints] * enemyCoefficient.HealthPoint)
            .AddMaxHealthPoints(baseStats[StatEnum.MaxHeathPoints] * enemyCoefficient.HealthPoint)
            .AddArmor(baseStats[StatEnum.Armor])
            .AddRotationSpeed(baseStats[StatEnum.RotationSpeed])
            .AddRound(round)
            .AddPlayerId(playerId)
            .AddAge(0)
            .AddGold(10)
            .With(x => x.isCanRaycast = true)
            .With(x => x.isEnemy = true)
            .With(x => x.isTarget = true)
            .With(x => x.isTurnedAlongDirection = true)
            .With(x => x.isMovementAvailable = true)
            .With(x => x.isHuman = isHuman, when: isHuman)
            .With(x => x.isFlyable = config.IsFlyable, when: config.IsFlyable);
    }

    public GameEntity CreatePlayer(bool isHuman, int index)
    {
        var player = CreateGameEntity
                .Empty()
                .AddId(_identifierService.Next())
                .AddGameLoopStateEnum(GameLoopStateEnum.PlayerAbility)
                .AddSpiritPlaced(0)
                .AddLevel(1)
                .AddRound(1)
                .AddCurrentHealthPoints(100)
                .AddMaxHealthPoints(100)
                .AddTotalGameTime(0)
                .AddRoundTimer(0)
                .AddIndex(index)
                .With(x => x.isPlayer = true)
                .With(x => x.isHuman = true, when: isHuman)
                .With(x => x.AddGold(350), when: isHuman)
            ;

        if (isHuman)
        {
            _playerAbilityFactory.CreatePlayerSwapAbility(player.Id);
            _playerAbilityFactory.CreateHealThroneAbility(player.Id);
            _playerAbilityFactory.CreateTimeLapseAbility(player.Id);
        }

        return player;
    }

    public void CreateMainEntity()
    {
        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddEnemySpawned(0)
            .AddRound(0)
            .With(x => x.isGameMain = true)
            ;
    }

    public void CreateRoundComplete(
        int playerId,
        int spawnerRound,
        int maxEnemies
    )
    {
        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPlayerId(playerId)
            .AddEnemiesKilled(0)
            .AddEnemiesPerRound(maxEnemies)
            .AddRound(spawnerRound)
            .With(x => x.isRoundComplete = true)
            ;
    }

    public GameEntity CreateLeftMouseClick()
    {
        return CreateGameEntity
                .Empty()
                .AddId(_identifierService.Next())
                .With(x => x.isDestructed = true)
                .With(x => x.isLeftMouseButtonClick = true)
            ;
    }

    public void CreateCancelSelectionRequest()
    {
        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .With(x => x.isCancelSelectionRequest = true)
            .With(x => x.isDestructed = true)
            ;
    }

    public void CreateSwapAbilityFinishRequest(AbilityEnum abilityEnum, int firstTower, int secondTower)
    {
        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddAbilityUsingFinishedEvent(abilityEnum)
            .AddSwapFirstTowerSelected(firstTower)
            .AddSwapSecondTowerSelected(secondTower)
            .With(x => x.isSwapFinishRequest = true)
            .With(x => x.isDestructed = true)
            ;
    }
}