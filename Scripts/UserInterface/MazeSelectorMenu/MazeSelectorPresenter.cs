using System;
using Game.GameMainFeature;
using Infrastructure.States.GameStates;
using Infrastructure.States.StateMachine;
using Services.ProjectData;
using Services.StaticData;
using Tools.MazeDesigner;
using Zenject;

namespace UserInterface.MazeSelectorMenu
{
    public class MazeSelectorPresenter : Presenter<MazeSelectorView>, IInitializable
    {
        private const string BattleSceneName = "Gameplay";

        private readonly IProjectDataService _projectDataService;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameStateMachine _stateMachine;
        private GameModeEnum _gameModeEnum;

        public event Action SettingPanelActivated;
        
        public MazeSelectorPresenter(
            MazeSelectorView mazeSelectorView,
            IGameStateMachine stateMachine,
            IProjectDataService projectDataService,
            IStaticDataService staticDataService) : base(mazeSelectorView)
        {
            _stateMachine = stateMachine;
            _projectDataService = projectDataService;
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            Hide();
        }

        public void Enable(GameModeEnum gameModeEnum)
        {
            if (gameModeEnum == GameModeEnum.None)
                throw new ArgumentOutOfRangeException(nameof(gameModeEnum), gameModeEnum, null);
           
            _gameModeEnum = gameModeEnum;

            View.StartButtonPressed += OnStartButtonViewPressed;
            View.ExitButtonPressed += OnExitButtonViewPressed;
            View.SettingButtonPressed += ShowSettingsPanel;
            
            FillView(gameModeEnum);

            Show();
        }

        public void Disable()
        {
            View.StartButtonPressed -= OnStartButtonViewPressed;
            View.ExitButtonPressed -= OnExitButtonViewPressed;
            View.SettingButtonPressed -= ShowSettingsPanel;

            Hide();
        }

        private void ShowSettingsPanel() => 
            SettingPanelActivated?.Invoke();

        private void OnStartButtonViewPressed()
        {
            Hide();

            _projectDataService.SetGameMode(_gameModeEnum);
            EnterBattleLoadingState();
        }

        private void OnMazeChosen(MazeDataSO mazeData) =>
            _projectDataService.SetMazePlan(mazeData);

        private void OnExitButtonViewPressed() =>
            BackToMainMenu();

        private void BackToMainMenu()
        {
            Disable();
            _stateMachine.Enter<LoadingHomeScreenState>();
        }

        private void FillView(GameModeEnum gameModeEnum)
        {
            MazeViewConfig mazeViewConfig = _staticDataService.GetMazeViewConfig(gameModeEnum);
            
            for (int i = 0; i < View.MazeViewTypes.Count; i++)
            {
                if (i > mazeViewConfig.wayBuildVariants.Count - 1)
                    break;

                View.MazeViewTypes[i].Image.sprite = mazeViewConfig.wayBuildVariants[i].Sprite;

                if (mazeViewConfig.wayBuildVariants[i].IsReady)
                {
                    int indexToWrite = i;
                    View.MazeViewTypes[i].SelectButton.interactable = true;
                    
                    View.MazeViewTypes[i].SelectButton.onClick.AddListener(() => 
                        OnMazeChosen(mazeViewConfig.wayBuildVariants[indexToWrite].Maze));
                    
                    View.MazeViewTypes[i].SelectButton.onClick.AddListener(() =>
                        View.StartButton.interactable = true);
                }
                else
                {
                    View.MazeViewTypes[i].SelectButton.interactable = false;
                }
            }
            
            SetFirstButtonActive(mazeViewConfig);
        }

        private void SetFirstButtonActive(MazeViewConfig mazeViewConfig)
        {
            if (mazeViewConfig.wayBuildVariants[0].IsReady)
            {
                OnMazeChosen(mazeViewConfig.wayBuildVariants[0].Maze);
                View.StartButton.interactable = true;
            }
            else
            {
                View.StartButton.interactable = false;
            }
        }

        private void EnterBattleLoadingState() =>
            _stateMachine.Enter<LoadingBattleState, string>(BattleSceneName);
    }
}