using Entitas;
using Infrastructure.States.GameStates;
using Infrastructure.States.StateMachine;
using Services.ProjectData;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay.CheatsPanel
{
    public class CheatsPanelPresenter :
        Presenter<CheatsPanelView>,
        IInitializable
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IGroup<GameEntity> _humans;
        private readonly IGroup<GameEntity> _enemies;
        private readonly IProjectDataService _projectDataService;
        public CheatsPanelPresenter(
            CheatsPanelView view,
            IGameStateMachine stateMachine,
            GameContext gameContext, IProjectDataService projectDataService) : base(view)
        {
            _stateMachine = stateMachine;
            _projectDataService = projectDataService;

            _humans = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.Human
                ));

            _enemies = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Enemy
                ));
        }

        public void Initialize()
        {
            View.RestartButton.onClick.AddListener(() => { _stateMachine.Enter<RestartState>(); }
            );

            View.HealButton.onClick.AddListener(() =>
                {
                    foreach (GameEntity human in _humans.GetEntities())
                        human.ReplaceCurrentHealthPoints(100);
                    
                    _projectDataService.CheatsUsed++;
                })
                ;

            View.MoneyButton.onClick.AddListener(() =>
            {
                foreach (GameEntity human in _humans.GetEntities())
                    human.ReplaceGold(1000);
                
                _projectDataService.CheatsUsed++;
            });

            View.KillButton.onClick.AddListener(() =>
            {
                foreach (GameEntity enemy in _enemies.GetEntities())
                foreach (GameEntity human in _humans.GetEntities())
                {
                    if (enemy.PlayerId == human.Id)
                        enemy.ReplaceCurrentHealthPoints(0);
                }
                
                _projectDataService.CheatsUsed++;
            });
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }
    }
}