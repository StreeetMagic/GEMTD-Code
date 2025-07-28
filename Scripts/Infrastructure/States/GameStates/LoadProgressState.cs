using Infrastructure.States.StateInfrastructure;
using Infrastructure.States.StateMachine;
using Services.SaveLoadServices;
using UserInterface.SettingsMenu;

namespace Infrastructure.States.GameStates
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISaveLoadService _saveLoadService;
        private readonly SettingsMenuPresenter _settingsMenuPresenter;

        public LoadProgressState(
            IGameStateMachine stateMachine,
            ISaveLoadService saveLoadService,
            SettingsMenuPresenter settingsMenuPresenter
        )
        {
            _saveLoadService = saveLoadService;
            _settingsMenuPresenter = settingsMenuPresenter;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _saveLoadService.ProgressReaders.Add(_settingsMenuPresenter);

            InitializeProgress();

            _stateMachine.Enter<LoadingHomeScreenState>();
        }

        private void InitializeProgress()
        {
            if (_saveLoadService.HasSavedProgress)
                _saveLoadService.LoadProgress();
            else
                CreateNewProgress();
        }

        private void CreateNewProgress()
        {
            _saveLoadService.LoadProgress();
        }

        public void Exit()
        {
        }
    }
}