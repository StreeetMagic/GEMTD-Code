using System;
using System.Collections.Generic;
using Entitas;
using Game;
using Game.Towers;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay
{
    public class TowerMergePanelPresenter :
        Presenter<TowerMergePanelView>,
        IInitializable
    {
        private IGroup<GameEntity> _humans;
        private readonly GameContext _gameContext;
        private readonly IStaticDataService _staticDataService;
        private readonly GameEntityFactories _gameEntityFactories;

        private bool _isPanelActive;

        private IGroup<GameEntity> _timeLapseRequests;

        public event Action Closed;
        public event Action TowerChosen;

        protected TowerMergePanelPresenter(
            TowerMergePanelView view,
            GameContext gameContext,
            IStaticDataService staticDataService, GameEntityFactories gameEntityFactories)
            : base(view)
        {
            _gameContext = gameContext;
            _staticDataService = staticDataService;
            _gameEntityFactories = gameEntityFactories;
        }

        public void Initialize()
        {
            _humans = _gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.Human,
                    GameMatcher.GameLoopStateEnum
                ));

            _timeLapseRequests = _gameContext.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.TimeLapseRequest
                    )
            );
        }

        public void Enable()
        {
            _timeLapseRequests.OnEntityAdded += OnTimeLapseRequestAdded;
        }

        private void OnTimeLapseRequestAdded(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
        {
            Hide();
        }

        public void Disable()
        {
        }

        public void ShowPanel()
        {
            _isPanelActive = true;
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();

            if (_isPanelActive)
            {
                _isPanelActive = false;
            }
        }

        public void Activate(GameEntity selectedSpirit)
        {
            if (!IsInChooseSpiritState())
                return;

            var mergeVariantsCount = selectedSpirit.MergeVariants.Count;
            var upgradeActions = new List<Action>();
            var upgradeTowerTypes = new List<TowerEnum>();

            for (int i = 0; i < mergeVariantsCount; i++)
            {
                TowerEnum targetEnum = selectedSpirit.MergeVariants[i];
                upgradeTowerTypes.Add(targetEnum);

                TowerEnum localTarget = targetEnum;

                upgradeActions.Add(() => OnOneHitUpgrade(selectedSpirit, localTarget));
            }
            
            var selectedSpiritConfig = _staticDataService.GetTowerConfig(selectedSpirit.TowerEnum);

            View.SelectButtonText.text = selectedSpirit.TowerEnum.ToString();
            View.SelectButtonImage.sprite = selectedSpiritConfig.Sprite;
            View.SelectButtonDescription.text = selectedSpiritConfig.Description;
            View.SelectButtonView.SetColor(selectedSpirit);

            bool canDowngrade =
                selectedSpiritConfig.DowngradeTo != TowerEnum.None;

            View.DowngradeButton.gameObject.SetActive(false);

            View.SelectButton.onClick.RemoveAllListeners();
            View.SelectButton.onClick.AddListener(OnSelect);

            View.DowngradeButton.onClick.RemoveAllListeners();
            View.DowngradeButton.onClick.AddListener(Hide);

            View.BackButton.onClick.RemoveAllListeners();
            View.BackButton.onClick.AddListener(OnBack);

            SubscribeActionsToButtons(upgradeActions, upgradeTowerTypes);

            ShowPanel();

            return;

            void OnSelect()
            {
                TowerChosen?.Invoke();
                _gameEntityFactories.CreateSpiritSelectRequest(selectedSpirit);

                Hide();
                selectedSpirit.isDestructed = true;
            }

            void OnBack()
            {
                Closed?.Invoke();
                Hide();
            }
        }

        private void SubscribeActionsToButtons(List<Action> actions, List<TowerEnum> variants)
        {
            int count = Mathf.Min(View.UpgradeButton.Length, actions.Count, variants.Count);

            for (int i = 0; i < View.UpgradeButton.Length; i++)
            {
                var button = View.UpgradeButton[i];
                var text = View.Texts[i];
                var image = View.Images[i];
                var description = View.Descriptions[i];
                var buttonView = View.ButtonViews[i];

                bool active = i < count;

                button.gameObject.SetActive(active);
                text.gameObject.SetActive(active);
                image.gameObject.SetActive(active);
                description.gameObject.SetActive(active);

                button.onClick.RemoveAllListeners();

                if (!active)
                    continue;

                buttonView.SetColor(variants[i].ToString());

                int index = i;

                button.onClick.AddListener(() =>
                {
                    if (index < actions.Count)
                        actions[index]?.Invoke();
                });

                text.text = variants[index].ToString();
                image.sprite = _staticDataService.GetTowerConfig(variants[index]).Sprite;
                description.text = _staticDataService.GetTowerConfig(variants[index]).Description;
            }
        }

        private void OnOneHitUpgrade(GameEntity selectedSpirit, TowerEnum targetTowerType)
        {
            _gameEntityFactories.CreateSpiritsMergeRequest(selectedSpirit, targetTowerType);

            Hide();
            selectedSpirit.isDestructed = true;
        }

        private bool IsInChooseSpiritState()
        {
            foreach (var human in _humans)
            {
                if (human.GameLoopStateEnum == GameLoopStateEnum.ChooseSpirit)
                    return true;
            }

            return false;
        }
    }
}