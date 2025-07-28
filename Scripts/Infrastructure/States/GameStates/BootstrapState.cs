using Infrastructure.States.StateInfrastructure;
using Infrastructure.States.StateMachine;
using Services.StaticData;
using UserInterface;

namespace Infrastructure.States.GameStates
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly CurtainPresenter _curtainPresenter;

        public BootstrapState(
            IGameStateMachine stateMachine,
            IStaticDataService staticDataService,
            CurtainPresenter curtainPresenter
        )
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _curtainPresenter = curtainPresenter;
        }

        public void Enter()
        {
            _staticDataService.LoadAll();
            _stateMachine.Enter<LoadProgressState>();
            _curtainPresenter.Show();
        }

        public void Exit()
        {
            _curtainPresenter.Hide();
        }
    }
}