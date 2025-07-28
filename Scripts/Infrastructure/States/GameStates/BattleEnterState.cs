using Infrastructure.States.StateInfrastructure;
using Infrastructure.States.StateMachine;
using Services.ProjectData;
using Services.SystemsFactoryServices;
using Zenject;

namespace Infrastructure.States.GameStates
{
    public class BattleEnterState : IState, ITickable
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISystemFactory _systems;
        private readonly IProjectDataService _projectDataService;

        public BattleEnterState(
            IGameStateMachine stateMachine, IProjectDataService projectDataService)
        {
            _stateMachine = stateMachine;
            _projectDataService = projectDataService;
        }

        public void Enter()
        {
            _stateMachine.Enter<BattleLoopState>();
            _projectDataService.CheatsUsed = 0;
        }

        public void Exit()
        {
        }

        public void Tick()
        {
        }
    }
}