using Entitas;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay.InfoPanel
{
    public class InfoPanelPresenter :
        Presenter<InfoPanelView>,
        IInitializable,
        IGoldListener
    {
        private readonly GameContext _gameContext;

        private IGroup<GameEntity> _humans;

        protected InfoPanelPresenter(InfoPanelView view, GameContext gameContext) : base(view)
        {
            _gameContext = gameContext;
        }

        public void Initialize()
        {
            _humans = _gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.Human
                ));
        }

        public void Enable()
        {
            foreach (var human in _humans)
            {
                human.AddGoldListener(this);
                OnGold(human, human.gold.Value);
            }
        }

        public void Disable()
        {
            foreach (var human in _humans)
                human.RemoveGoldListener(this);
        }

        public void OnGold(GameEntity entity, int value)
        {
            View.Text.text = value.ToString(); 
        }
    }
}