using Game.GameMainFeature;
using Infrastructure.States.StateInfrastructure;
using Services.SystemsFactoryServices;
using UserInterface.GameplayHeadsUpDisplay;

namespace Infrastructure.States.GameStates
{
    public class BattleLoopState : IState, IUpdateable
    {
        private readonly ISystemFactory _systems;
        private readonly GameContext _gameContext;
        private readonly GameplayHeadsUpDisplayPresenter _gameplayHeadsUpDisplayPresenter;

        private GameplayFeature _gameplayFeature;

        public BattleLoopState(
            ISystemFactory systems,
            GameContext gameContext,
            GameplayHeadsUpDisplayPresenter gameplayHeadsUpDisplayPresenter
        )
        {
            _systems = systems;
            _gameContext = gameContext;
            _gameplayHeadsUpDisplayPresenter = gameplayHeadsUpDisplayPresenter;
        }

        public void Enter()
        {
            _gameplayFeature = _systems.Create<GameplayFeature>();
            _gameplayFeature.Initialize();

            _gameplayHeadsUpDisplayPresenter.Enable();
        }

        public void Update()
        {
            if (_gameplayFeature == null)
                return;

            _gameplayFeature.Execute();

            _gameplayFeature?.Cleanup();
        }

        public void Exit()
        {
            _gameplayFeature.DeactivateReactiveSystems();
            _gameplayFeature.ClearReactiveSystems();

            DestructEntities();

            _gameplayFeature.Cleanup();
            _gameplayFeature.TearDown();
            _gameplayFeature = null;

            _gameplayHeadsUpDisplayPresenter.Disable();
        }

        private void DestructEntities()
        {
            foreach (GameEntity entity in _gameContext.GetEntities())
                entity.isDestructed = true;
        }
    }
}