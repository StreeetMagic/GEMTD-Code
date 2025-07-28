using Infrastructure.Loading;
using Infrastructure.SceneLoading;
using Infrastructure.States.StateInfrastructure;
using Infrastructure.States.StateMachine;

namespace Infrastructure.States.GameStates
{
    public class RestartState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameStateMachine _stateMachine;

        public RestartState(ISceneLoader sceneLoader, IGameStateMachine stateMachine)
        {
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _stateMachine.Enter<LoadingHomeScreenState>();
        }

        public void Exit()
        {
        }
    }
}