using Entitas;
using Game.Extensions;
using Services.Times;
using UserInterface.GameplayHeadsUpDisplay.FinishPanel;

namespace Game.GameFinish
{
    public class PlayerDefeatSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _humans;
        private readonly ITimeService _timeService;
        private readonly FinishPanelPresenter _finishPanelPresenter;
        private bool _finished;

        public PlayerDefeatSystem(GameContext game,
            ITimeService timeService,
            FinishPanelPresenter finishPanelPresenter)
        {
            _humans = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.Human,
                    GameMatcher.CurrentHealthPoints
                ));
            _timeService = timeService;
        }

        public void Execute()
        {
            if (_finished)
                return;

            foreach (var human in _humans)
            {
                if (human.CurrentHealthPoints <= 0)
                {
                    _finished = true;
                    human.With(x => x.isGameFinish = true);
                    _timeService.Pause();

                    break;
                }
            }
        }
    }
}