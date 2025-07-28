using System;
using Game.GameMainFeature;
using Infrastructure.States.GameStates;
using Infrastructure.States.StateMachine;
using Services.ProjectData;
using UnityEngine;
using Zenject;

namespace UserInterface.MainMenu
{
    public class MainMenuPresenter : Presenter<MainMenuView>, IInitializable
    {
        private readonly IProjectDataService _projectDataService;
        private readonly IGameStateMachine _stateMachine;

        public event Action SettingPanelActivated;

        public MainMenuPresenter(
            MainMenuView view,
            IProjectDataService projectDataService,
            IGameStateMachine stateMachine
        ) : base(view)
        {
            _projectDataService = projectDataService;
            _stateMachine = stateMachine;
        }

        public void Initialize()
        {
            View.Button1.onClick.AddListener(() =>
            {
                EnterMazeSelectorState(GameModeEnum.SingleSmall);
            });

            View.Button2.onClick.AddListener(() =>
            {
                EnterMazeSelectorState(GameModeEnum.RaceSmall);
            });

            View.Button3.onClick.AddListener(() =>
            {
                EnterMazeSelectorState(GameModeEnum.SingleLarge);
            });

            View.Button4.onClick.AddListener(() =>
            {
                EnterMazeSelectorState(GameModeEnum.SingleLargeDouble);
            });

            View.Button5.onClick.AddListener(() =>
            {
                EnterMazeSelectorState(GameModeEnum.SingleLargeTriple);
            });

            View.Button6.onClick.AddListener(() =>
            {
                EnterMazeSelectorState(GameModeEnum.SingleLargeQuad);
            });
            
            View.ExitButton.onClick.AddListener(() =>
            {
                Debug.LogError("Так называемый выход");
                Application.Quit();
            });

            View.SettingsButton.onClick.AddListener(() =>
            {
                SettingPanelActivated?.Invoke();
            });
            
            Hide();
        }

        public void Enable()
        {
            _projectDataService.ResetGameModes();
            Show();
        }

        public void Disable() => 
            Hide();

        private void EnterMazeSelectorState(GameModeEnum gameModeEnum) =>
            _stateMachine.Enter<MazeSelectorState, GameModeEnum>(gameModeEnum);
    }
}