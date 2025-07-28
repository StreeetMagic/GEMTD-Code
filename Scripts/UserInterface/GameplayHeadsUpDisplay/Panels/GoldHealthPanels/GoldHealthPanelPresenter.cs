using System.Globalization;
using Entitas;

namespace UserInterface.GameplayHeadsUpDisplay.GoldHealthPanels
{
    public class GoldHealthPanelPresenter : Presenter<GoldHealthPanelView>, ICurrentHealthPointsListener, IGoldListener
    {
        private readonly IGroup<GameEntity> _humans;

        public GoldHealthPanelPresenter(
            GoldHealthPanelView view,
            GameContext gameContext
        ) : base(view)
        {
            _humans = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.Human
                ));
        }

        public void Enable()
        {
            View.Show();

            foreach (GameEntity human in _humans.GetEntities())
            {
                human.AddCurrentHealthPointsListener(this);
                human.AddGoldListener(this);

                OnCurrentHealthPoints(human, human.CurrentHealthPoints);
                OnGold(human, human.Gold);
            }
        }

        public void Disable()
        {
            View.Hide();

            foreach (GameEntity human in _humans.GetEntities())
            {
                human.RemoveCurrentHealthPointsListener(this);
                human.RemoveGoldListener(this);
            }
        }

        public void OnCurrentHealthPoints(GameEntity entity, float value)
        {
            View.HealthText.text = $"{value}";
        }

        public void OnGold(GameEntity entity, int value)
        {
            View.GoldText.text = $"{value}";
        }
    }
}