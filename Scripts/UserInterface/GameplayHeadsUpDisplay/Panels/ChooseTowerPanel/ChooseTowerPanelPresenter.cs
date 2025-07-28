using System;
using System.Collections.Generic;
using Entitas;
using Game;
using Services.CoroutineRunners;
using Services.StaticData;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay
{
    public class ChooseTowerPanelPresenter :
        Presenter<ChooseTowerPanelView>,
        IGameLoopStateEnumListener,
        IInitializable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly GameplayHeadsUpDisplayView _gameplayHeadsUpDisplayView;
        private readonly List<GameEntity> _result = new();
        private readonly GameContext _gameContext;
        private readonly ICoroutineRunner _coroutineRunner;
        private IGroup<GameEntity> _spirits;
        private IGroup<GameEntity> _humans;
        
        private bool _isPanelActive;
        private int _queuedRequests;
        private bool _waitingForMerge;
        private bool _isInputBlocked = true;
        
        public event Action<GameEntity> ButtonClicked;
        public event Action Enabled;

        protected ChooseTowerPanelPresenter(
            ChooseTowerPanelView view,
            GameContext gameContext,
            IStaticDataService staticDataService,
            GameplayHeadsUpDisplayView gameplayHeadsUpDisplayView,
            ICoroutineRunner coroutineRunner
        ) : base(view)
        {
            _gameContext = gameContext;
            _staticDataService = staticDataService;
            _gameplayHeadsUpDisplayView = gameplayHeadsUpDisplayView;
            _coroutineRunner = coroutineRunner;
        }
        
        public void Initialize()
        {
            _spirits = _gameContext.GetGroup(GameMatcher.TowerSpirit);

            _humans = _gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.Human,
                    GameMatcher.GameLoopStateEnum
                ));
        }

        public void Enable()
        {
            UpdateButtons();

            foreach (var human in _humans)
                human.AddGameLoopStateEnumListener(this);

            _spirits.OnEntityAdded += OnSpiritAdded;

            Enabled?.Invoke();
        }

        public void Disable()
        {
            foreach (var human in _humans)
                human.RemoveGameLoopStateEnumListener(this);

            _spirits.OnEntityAdded -= OnSpiritAdded;
        }
        
        private void ShowPanel()
        {
            _isPanelActive = true;
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            _isPanelActive = false;

            if (_waitingForMerge)
                return;

            if (_queuedRequests > 0)
            {
                _queuedRequests--;
            }
        }

        private void HideSilently()
        {
            base.Hide();
            _isPanelActive = false;
        }

        public void MergeWindowClosed()
        {
            if (!_waitingForMerge)
                return;

            _waitingForMerge = false;

            if (_queuedRequests > 0)
            {
                _queuedRequests--;
                ShowPanel();
                UpdateButtons();
            }
        }

        public void OnGameLoopStateEnum(GameEntity entity, GameLoopStateEnum value)
        {
            var condition = value
                    is GameLoopStateEnum.ChooseSpirit
                    or GameLoopStateEnum.PlayerAbility
                    or GameLoopStateEnum.PlaceSpirit
                ;

            if (condition)
            {
                if (value == GameLoopStateEnum.PlaceSpirit)
                {
                    if (_isPanelActive)
                    {
                        _queuedRequests++;

                        return;
                    }

                    ShowPanel();
                }

                UpdateButtons();
            }
            else
            {
                HideSilently();
            }
        }

        private void UpdateButtons()
        {
            _isInputBlocked = true;
            HideSilently();
            
            List<GameEntity> visibleSpirits = GetVisibleSpirits();

            for (var i = 0; i < View.TowerButtons.Length; i++)
            {
                var buttonView = View.TowerButtons[i];
                var button = buttonView.Button;
                var active = i < visibleSpirits.Count;

                if (buttonView.Button.gameObject.activeSelf != active)
                {
                    buttonView.Button.gameObject.SetActive(active);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(_gameplayHeadsUpDisplayView.rectTransform);
                }

                buttonView.Button.onClick.RemoveAllListeners();

                if (!active)
                    continue;

                ShowPanel();

                GameEntity spirit = visibleSpirits[i];
                buttonView.Text.text = spirit.TowerEnum.ToString();
                buttonView.Description.text = _staticDataService.GetTowerConfig(spirit.TowerEnum).Description;
                buttonView.BackGroundImage.color = Color.white;
                buttonView.BackGroundImage.sprite = _staticDataService.GetTowerConfig(spirit.TowerEnum).Sprite;
                
                buttonView.SetColor(spirit);

                button.onClick.AddListener(() =>
                {
                    if (_isInputBlocked)
                        return;
                        
                    if (GetVisibleSpirits().Count < 5)
                        return;

                    _waitingForMerge = true;
                    Hide();
                    ButtonClicked?.Invoke(spirit);
                });
            }
            
            _coroutineRunner.StartCoroutine(EnableInputAfterDelay());
        }

        private System.Collections.IEnumerator EnableInputAfterDelay()
        {
            yield return new WaitForSeconds(1.5f);
            _isInputBlocked = false;
        }

        private List<GameEntity> GetVisibleSpirits()
        {
            _result.Clear();

            foreach (var human in _humans)
            foreach (var spirit in _spirits)
            {
                if (spirit.PlayerId != human.Id)
                    continue;

                if (spirit.isDestructed)
                    continue;

                _result.Add(spirit);

                if (_result.Count >= _staticDataService.ProjectConfig.TowersPerRound)
                    break;
            }

            return _result;
        }

        private void OnSpiritAdded(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
        {
            if (!entity.isTowerSpirit)
                return;
            
            if (_isPanelActive || _waitingForMerge)
            {
                _queuedRequests++;
                return;
            }
            
            UpdateButtons();
        }
    }
}