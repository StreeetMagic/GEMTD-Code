using Game.Battle.Configs;
using Game.Entity;
using Game.Extensions;
using Game.Towers;
using Services.Identifiers;
using Services.StaticData;
using UnityEngine;

public class VisualEffectFactory
{
    private readonly IIdentifierService _identifierService;
    private readonly IStaticDataService _staticDataService;
    private readonly GameContext _game;

    public VisualEffectFactory(
        IIdentifierService identifierService,
        IStaticDataService staticDataService,
        GameContext game
    )
    {
        _identifierService = identifierService;
        _staticDataService = staticDataService;
        _game = game;
    }

    public void CreateExplosionVisualEffect(
        Vector3 position,
        int producerId
    )
    {
        var entity = _game.GetEntityWithId(producerId);
        AbilitySetup setup = _staticDataService.GetTowerConfig(entity.TowerEnum).BasicAttackSetup;

        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPrefab(setup.ExplosionPrefab)
            .AddWorldPosition(position)
            .AddSelfDestructTimer(2f)
            .With(x => x.isExplosion = true)
            ;
    }
    
    public void CreatePlaceSpiritVisualEffect(
        Vector3 position,
    TowerEnum tower)
    {
        var config = _staticDataService.GetTowerConfig(tower);
    
        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPrefab(config.SpiritPlacePrefab)
            .AddWorldPosition(position + new Vector3(0, 0.05f))
            .AddRotation(Quaternion.identity)
            .AddSelfDestructTimer(2f)
            ;
    }

    public void CreateMuzzleFlashEffect(
        Vector3 position,
        Quaternion rotation,
        int producerId
    )
    {
        var entity = _game.GetEntityWithId(producerId);
        AbilitySetup setup = _staticDataService.GetTowerConfig(entity.TowerEnum).BasicAttackSetup;

        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPrefab(setup.MuzzleFlashPrefab)
            .AddWorldPosition(position)
            .AddRotation(rotation)
            .AddProducerId(producerId)
            .AddSelfDestructTimer(2f)
            .With(x => x.isMuzzleFlash = true)
            ;
    }

    public void CreateCoinDropsEffect(Vector3 position,
        Quaternion rotation,
        int producerId)
    {
        CreateGameEntity
            .Empty()
            .AddId(_identifierService.Next())
            .AddPrefab(_staticDataService.ProjectConfig.CoinDropsEffect)
            .AddWorldPosition(position)
            .AddRotation(rotation)
            .AddProducerId(producerId)
            .AddSelfDestructTimer(2f)
            ;
    }
}