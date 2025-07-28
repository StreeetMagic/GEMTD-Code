using Services.Cameras;
using Services.ViewContainerProviders;
using UnityEngine;
using Zenject;

namespace Infrastructure.Scenes.Hubs
{
    public class GameplayInstaller : MonoInstaller, IInitializable
    {
        [Inject]
        private ViewContainerProvider _viewContainerProvider;

        [Inject]
        private ICameraProvider _cameraProvider;

        public Transform Common;
        public Transform Blocks;
        public Transform Enemies;
        public Transform Map;
        public Camera Camera;

        public override void InstallBindings()
        {
             Container.BindInterfacesAndSelfTo<GameplayInstaller>().FromInstance(this).AsSingle().NonLazy();
        }

        public void Initialize()
        {
            _cameraProvider.Camera = Camera;
            _viewContainerProvider.CommonContainer = Common;
            _viewContainerProvider.BlockContainer = Blocks;
            _viewContainerProvider.EnemyContainer = Enemies;
            _viewContainerProvider.MapContainer = Map;
        }
    }
}