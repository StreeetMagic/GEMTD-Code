using Services.ViewContainerProviders;
using UnityEngine;
using Zenject;

namespace Game.Factory
{
    public class GameEntityViewFactory : IGameEntityViewFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly ViewContainerProvider _viewContainerProvider;

        public GameEntityViewFactory(
            IInstantiator instantiator,
            ViewContainerProvider viewContainerProvider
        )
        {
            _instantiator = instantiator;
            _viewContainerProvider = viewContainerProvider;
        }

        public void Create(GameEntity entity, Vector3 at)
        {
            Transform parent = _viewContainerProvider.CommonContainer;
            
            if (entity.isBlock)
                parent = _viewContainerProvider.BlockContainer;

            if (entity.isEnemy)
                parent = _viewContainerProvider.EnemyContainer;

            GameEntityView view = _instantiator.InstantiatePrefabForComponent<GameEntityView>(
                entity.Prefab,
                position: at,
                Quaternion.identity,
                parentTransform: parent
            );

            view.transform.SetParent(parent);

            view.SetEntity(entity);
        }
    }
}