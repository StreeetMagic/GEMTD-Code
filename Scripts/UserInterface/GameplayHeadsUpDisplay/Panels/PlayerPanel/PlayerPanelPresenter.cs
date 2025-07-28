using Entitas;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay.PlayerPanel
{
    public class PlayerPanelPresenter :
        Presenter<PlayerPanelView>,
        ICurrentHealthPointsListener,
        IInitializable
    {
        private readonly IStaticDataService _staticDataService;
        private IGroup<GameEntity> _humans;
        private readonly GameContext _gameContext;

        protected PlayerPanelPresenter(PlayerPanelView view, IStaticDataService staticDataService,
            GameContext gameContext) : base(view)
        {
            _staticDataService = staticDataService;
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
        }

        public void Enable()
        {
            _humans.OnEntityAdded += OnHumanAdded;
        }

        public void Disable()
        {
            _humans.OnEntityAdded -= OnHumanAdded;
        }

        public void OnCurrentHealthPoints(GameEntity entity, float value)
        {
            float maxHp = _staticDataService.ProjectConfig.MaxThroneHealthPoint;
            float normalized = value / maxHp;

            View.HealthBar.value = normalized;
            View.HealthText.text =
                $"{Mathf.RoundToInt(normalized * _staticDataService.ProjectConfig.MaxThroneHealthPoint)}%";
        }

        private void OnHumanAdded(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
        {
            entity.AddCurrentHealthPointsListener(this);

            if (entity.hasCurrentHealthPoints)
                OnCurrentHealthPoints(entity, entity.CurrentHealthPoints);
        }
    }
}