using Infrastructure.Loading;
using Infrastructure.SceneLoading;
using Infrastructure.States.StateInfrastructure;
using Infrastructure.States.StateMachine;

namespace Infrastructure.States.GameStates
{
    public class LoadingHomeScreenState : IState
    {
        private const string HomeScreenSceneName = "MainMenu";
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameStateMachine _stateMachine;

        public LoadingHomeScreenState(
            ISceneLoader sceneLoader,
            IGameStateMachine stateMachine
        )
        {
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _sceneLoader.LoadScene(HomeScreenSceneName, OnLoaded);
        }

        private void OnLoaded()
        {
            _stateMachine.Enter<HomeScreenState>();
        }

        public void Exit()
        {
        }
    }
}