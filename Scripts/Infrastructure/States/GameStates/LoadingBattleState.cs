using System;
using Game.GameMainFeature;
using Infrastructure.Loading;
using Infrastructure.SceneLoading;
using Infrastructure.States.StateInfrastructure;
using Infrastructure.States.StateMachine;
using Services.AssetProviders;
using Services.ProjectData;
using Services.ViewContainerProviders;
using UserInterface;
using Object = UnityEngine.Object;

namespace Infrastructure.States.GameStates
{
    public class LoadingBattleState : IPayloadState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IProjectDataService _projectDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly ViewContainerProvider _viewContainerProvider;
        private readonly CurtainPresenter _curtainPresenter;

        public LoadingBattleState(
            IGameStateMachine stateMachine,
            ISceneLoader sceneLoader,
            IProjectDataService projectDataService,
            IAssetProvider assetProvider,
            ViewContainerProvider viewContainerProvider,
            CurtainPresenter curtainPresenter
        )
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _projectDataService = projectDataService;
            _assetProvider = assetProvider;
            _viewContainerProvider = viewContainerProvider;
            _curtainPresenter = curtainPresenter;
        }

        public void Enter(string sceneName)
        {
            _curtainPresenter.Show();
            _sceneLoader.LoadScene(sceneName, EnterBattleLoopState);
        }

        private void EnterBattleLoopState()
        {
            _stateMachine.Enter<BattleEnterState>();
            CreateMap();
            _curtainPresenter.Hide();
        }

        private void CreateMap()
        {
            var mapContainer = _viewContainerProvider.MapContainer;

            var gameMode = _projectDataService.CurrentGameModeType;

            var map1 = _assetProvider.LoadAsset("Map17x1");
            var map2 = _assetProvider.LoadAsset("Map17x4");
            var map3 = _assetProvider.LoadAsset("Map37x1");

            switch (gameMode)
            {
                case GameModeEnum.SingleSmall:
                    Object.Instantiate(map1, mapContainer);
                    break;

                case GameModeEnum.RaceSmall:
                    Object.Instantiate(map2, mapContainer);
                    break;

                case GameModeEnum.SingleLarge:
                case GameModeEnum.SingleLargeDouble:
                case GameModeEnum.SingleLargeTriple:
                case GameModeEnum.SingleLargeQuad:
                    Object.Instantiate(map3, mapContainer);
                    break;

                case GameModeEnum.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Exit()
        {
        }
    }
}