using Entitas;
using Infrastructure.Loading;
using Infrastructure.States.GameStates;
using Infrastructure.States.StateMachine;
using Services.ProjectData;
using Services.Times;
using UnityEngine;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay.FinishPanel
{
    public class FinishPanelPresenter : Presenter<FinishPanelView>, IInitializable, IAnyGameFinishListener
    {
        private readonly FinishPanelView _view;
        private readonly GameContext _game;
        private readonly IGameStateMachine _stateMachine;
        private readonly ITimeService _timeService;
        private readonly IProjectDataService _projectDataService;
        private IGroup<GameEntity> _humans;
        private IGroup<GameEntity> _roundCompletes;
        private IGroup<GameEntity> _uniqueRealPlayers;
        private IGroup<GameEntity> _times;

        public FinishPanelPresenter(
            FinishPanelView view,
            GameContext game,
            IGameStateMachine stateMachine,
            ITimeService timeService, 
            IProjectDataService projectDataService
            ) : base(view)
        {
            _view = view;
            _game = game;
            _stateMachine = stateMachine;
            _timeService = timeService;
            _projectDataService = projectDataService;
        }

        public void Initialize()
        {
            Hide();
            
            _view.RestartButton.onClick.AddListener(Restart);

            _uniqueRealPlayers = _game.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Human,
                        GameMatcher.Player));

            _humans = _game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Human,
                    GameMatcher.Player
                ));

            _roundCompletes = _game.GetGroup(
                GameMatcher.AllOf(GameMatcher.PlayerId,
                    GameMatcher.EnemiesKilled,
                    GameMatcher.RoundComplete
                ));
        }

        public void Enable()
        {
            var player = GetRealPlayer();
            
            if (player != null)
                player.AddAnyGameFinishListener(this);
        }

        public void Disable()
        {
            var player = GetRealPlayer();
            
            if (player != null)
                player.RemoveAnyGameFinishListener(this);
        }
        
        private void Restart()
        {
            Hide();
            _timeService.UnPause();
            _stateMachine.Enter<RestartState>();
        }

        public void OnAnyGameFinish(GameEntity entity)
        {
            UpdateViewInfo();
            Show();
            
            if (_projectDataService.CheatsUsed > 0)
                View.CheatsText.text = "Cheats Used: " + _projectDataService.CheatsUsed;
            else
                View.CheatsText.text = "";
        }

        private void UpdateViewInfo()
        {
            foreach (GameEntity human in _humans)
            foreach (GameEntity roundComplete in _roundCompletes)
            {
                View.TopText.text = "Game Over!";

                string totalGameTime = GetTotalGameTime(human.TotalGameTime);

                View.DurationText.text = $"Game Time: {totalGameTime}";
                View.KillText.text = $"Kills: {roundComplete.EnemiesKilled.ToString()}";
                View.LevelText.text = $"Level: {human.Level.ToString()}";
            }
        }

        private string GetTotalGameTime(float value)
        {
            int seconds = Mathf.FloorToInt(value);
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            int secs = seconds % 60;

            return $"{hours:D2}:{minutes:D2}:{secs:D2}";
        }

        private GameEntity GetRealPlayer()
        {
            GameEntity[] allPlayers = _uniqueRealPlayers.GetEntities();

            if (allPlayers.Length == 0)
                return null;

            return allPlayers[0];
        }
    }
}