using Infrastructure.States.StateInfrastructure;
using Infrastructure.States.StateMachine;
using Services.AudioServices;
using Services.AudioServices.Sounds;
using UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel.SwapAbility;
using UserInterface.MainMenu;

namespace Infrastructure.States.GameStates
{
    public class HomeScreenState : IState, IUpdateable
    {
        private readonly GameContext _gameContext;
        private readonly MainMenuPresenter _mainMenuPresenter;
        private readonly AudioService _audioService;
        private readonly TowerSwapAbilityPanelPresenter _swapAbilityPanelPresenter;

        public HomeScreenState(
            GameContext gameContext,
            MainMenuPresenter mainMenuPresenter,
            AudioService audioService, 
            TowerSwapAbilityPanelPresenter swapAbilityPanelPresenter
            )
        {
            _gameContext = gameContext;
            _mainMenuPresenter = mainMenuPresenter;
            _audioService = audioService;
            _swapAbilityPanelPresenter = swapAbilityPanelPresenter;
        }

        public void Enter()
        {
            _mainMenuPresenter.Enable();
            _swapAbilityPanelPresenter.Hide();
            _audioService.PlayMusic(SoundEnum.Music);
        }

        public void Update()
        {
        }

        public void Exit()
        {
            _mainMenuPresenter.Disable();
            DestructEntities();
        }

        private void DestructEntities()
        {
            foreach (GameEntity entity in _gameContext.GetEntities())
                entity.isDestructed = true;
        }
    }
}