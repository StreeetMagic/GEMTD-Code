using Entitas;
using UnityEngine;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay.TimerPanel
{
    public class TimerPanelPresenter :
        Presenter<TimerPanelView>,
        IInitializable,
        IRoundTimerListener,
        IRoundListener,
        ITotalGameTimeListener
    {
        private readonly GameContext _gameContext;
        private IGroup<GameEntity> _spirits;
        private IGroup<GameEntity> _humans;
        private IGroup<GameEntity> _aliveEnemies;

        protected TimerPanelPresenter(TimerPanelView view, GameContext gameContext) : base(view)
        {
            _gameContext = gameContext;
        }

        public void Initialize()
        {
            _humans = _gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.Human,
                    GameMatcher.GameLoopStateEnum
                ));

            _aliveEnemies = _gameContext.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Enemy,
                        GameMatcher.Human
                    )
                    .NoneOf(
                        GameMatcher.Dead
                    ));
        }

        public void Enable()
        {
            foreach (var human in _humans)
            {
                human.AddRoundListener(this);
                human.AddTotalGameTimeListener(this);
                human.AddRoundTimerListener(this);

                if (human.hasTotalGameTime)
                    OnTotalGameTime(human, human.totalGameTime.Value);
            }

            _aliveEnemies.OnEntityAdded += OnAliveEnemyChanged;
            _aliveEnemies.OnEntityRemoved += OnAliveEnemyChanged;
        }

        public void Disable()
        {
            foreach (var human in _humans)
            {
                human.RemoveTotalGameTimeListener(this);
                human.RemoveRoundListener(this);
                human.RemoveRoundTimerListener(this);
            }

            _aliveEnemies.OnEntityAdded -= OnAliveEnemyChanged;
            _aliveEnemies.OnEntityRemoved -= OnAliveEnemyChanged;
        }

        public void OnRound(GameEntity entity, int value)
        {
            View.CurrentRound.text = $"{value}";
        }

        public void OnRoundTimer(GameEntity entity, float value)
        {
            int seconds = Mathf.FloorToInt(value);
            int minutes = seconds / 60;
            int secs = seconds % 60;

            View.CurrentRoundTimerText.text = $"{minutes:D2}:{secs:D2}";
        }

        public void OnTotalGameTime(GameEntity entity, float value)
        {
            int seconds = Mathf.FloorToInt(value);
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            int secs = seconds % 60;

            View.TotalGameTimeText.text = $"{hours:D2}:{minutes:D2}:{secs:D2}";
        }

        private void OnAliveEnemyChanged(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
        {
            View.AliveEnemies.text = $"x {group.count}";
        }
    }
}