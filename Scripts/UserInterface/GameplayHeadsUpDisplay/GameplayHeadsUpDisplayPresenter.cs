using System;
using Entitas;
using Game.Battle;
using Infrastructure.States.StateMachine;
using Services.StaticData;
using UnityEngine;
using UserInterface.GameplayHeadsUpDisplay.CheatsPanel;
using UserInterface.GameplayHeadsUpDisplay.FinishPanel;
using UserInterface.GameplayHeadsUpDisplay.GoldHealthPanels;
using UserInterface.GameplayHeadsUpDisplay.InfoPanel;
using UserInterface.GameplayHeadsUpDisplay.ObjectInfoPanel;
using UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel;
using UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel.SwapAbility;
using UserInterface.GameplayHeadsUpDisplay.PlayerPanel;
using UserInterface.GameplayHeadsUpDisplay.TimerPanel;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay
{
    public class GameplayHeadsUpDisplayPresenter :
        Presenter<GameplayHeadsUpDisplayView>,
        IInitializable
    {
        private readonly ChooseTowerPanelPresenter _chooseTowerPanelPresenter;
        private readonly TowerMergePanelPresenter _towerMergePanelPresenter;
        private readonly TimerPanelPresenter _timerPanelPresenter;
        private readonly PlayerPanelPresenter _playerPanelPresenter;
        private readonly PlayerAbilityPanelPresenter _playerAbilityPanelPresenter;
        private readonly CheatsPanelPresenter _cheatsPanelPresenter;
        private readonly InfoPanelPresenter _infoPanelPresenter;
        private readonly TowerSwapAbilityPanelPresenter _towerSwapAbilityPanelPresenter;
        private readonly PausePanelPresenter _pausePanelPresenter;
        private readonly FinishPanelPresenter _finishPanelPresenter;
        private readonly ObjectInfoPanelPresenter _objectInfoPanelPresenter;
        private readonly GoldHealthPanelPresenter _goldHealthPanelPresenter;

        private readonly GameContext _gameContext;
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;

        private IGroup<GameEntity> _spirits;
        private IGroup<GameEntity> _humans;
        private IGroup<GameEntity> _aliveEnemies;

        private float _tickTime;

        public event Action SettingPanelActivated;
        
        public GameplayHeadsUpDisplayPresenter(
            GameplayHeadsUpDisplayView view,
            ChooseTowerPanelPresenter chooseTowerPanelPresenter,
            TowerMergePanelPresenter towerMergePanelPresenter,
            TimerPanelPresenter timerPanelPresenter,
            PlayerPanelPresenter playerPanelPresenter,
            PlayerAbilityPanelPresenter playerAbilityPanelPresenter,
            CheatsPanelPresenter cheatsPanelPresenter,
            InfoPanelPresenter infoPanelPresenter,
            TowerSwapAbilityPanelPresenter towerSwapAbilityPanelPresenter,
            PausePanelPresenter pausePanelPresenter,
            FinishPanelPresenter finishPanelPresenter,
            ObjectInfoPanelPresenter objectInfoPanelPresenter,
            GameContext gameContext, 
            GoldHealthPanelPresenter goldHealthPanelPresenter
            ) : base(view)
        {
            _objectInfoPanelPresenter = objectInfoPanelPresenter;
            _chooseTowerPanelPresenter = chooseTowerPanelPresenter;
            _towerMergePanelPresenter = towerMergePanelPresenter;
            _timerPanelPresenter = timerPanelPresenter;
            _playerPanelPresenter = playerPanelPresenter;
            _playerAbilityPanelPresenter = playerAbilityPanelPresenter;
            _cheatsPanelPresenter = cheatsPanelPresenter;
            _infoPanelPresenter = infoPanelPresenter;
            _towerSwapAbilityPanelPresenter = towerSwapAbilityPanelPresenter;
            _pausePanelPresenter = pausePanelPresenter;
            _finishPanelPresenter = finishPanelPresenter;
            _gameContext = gameContext;
            _goldHealthPanelPresenter = goldHealthPanelPresenter;
        }

        public void Initialize()
        {
            Hide();
        }

        public void Enable()
        {
            Show();
            View.SettingButtonPressed += OnSettingButtonPressed;
            
            _gameContext.OnEntityDestroyed += OnEntityDestroyed;

            _chooseTowerPanelPresenter.ButtonClicked += OnChooseTowerButtonClicked;
            _chooseTowerPanelPresenter.Enabled += OnChooseTowerPanelEnabled;
            _towerMergePanelPresenter.Closed += OnTowerMergePanelClosed;
            _playerAbilityPanelPresenter.RequestToOpenAbilityPanel += OnRequestToOpenAbilityPanel;
            _towerSwapAbilityPanelPresenter.SwapDisabled += OnSwapDisabled;
            _towerMergePanelPresenter.TowerChosen += OnTowerChosen; 

            _chooseTowerPanelPresenter.Enable();
            _towerMergePanelPresenter.Enable();
            _timerPanelPresenter.Enable();
            _playerPanelPresenter.Enable();
            _playerAbilityPanelPresenter.Enable();
            _cheatsPanelPresenter.Enable();
            _infoPanelPresenter.Enable();
            _towerSwapAbilityPanelPresenter.Enable();
            _pausePanelPresenter.Enable();
            _finishPanelPresenter.Enable();
            _objectInfoPanelPresenter.Enable();
            _goldHealthPanelPresenter.Enable();
        }

        private void OnSettingButtonPressed() => 
            SettingPanelActivated?.Invoke();
        
        private void OnEntityDestroyed(IContext context, IEntity entity)
        {
        }

        public void Disable()
        {
            View.SettingButtonPressed -= OnSettingButtonPressed;

            _chooseTowerPanelPresenter.ButtonClicked -= OnChooseTowerButtonClicked;
            _chooseTowerPanelPresenter.Enabled -= OnChooseTowerPanelEnabled;
            _towerMergePanelPresenter.Closed -= OnTowerMergePanelClosed;
            _playerAbilityPanelPresenter.RequestToOpenAbilityPanel -= OnRequestToOpenAbilityPanel;
            _towerSwapAbilityPanelPresenter.SwapDisabled -= OnSwapDisabled;
            _towerMergePanelPresenter.TowerChosen -= OnTowerChosen; 

            Hide();

            _chooseTowerPanelPresenter.Disable();
            _towerMergePanelPresenter.Disable();
            _timerPanelPresenter.Disable();
            _playerPanelPresenter.Disable();
            _playerAbilityPanelPresenter.Disable();
            _cheatsPanelPresenter.Disable();
            _infoPanelPresenter.Disable();
            _towerSwapAbilityPanelPresenter.Disable();
            _pausePanelPresenter.Disable();
            _finishPanelPresenter.Disable();
            _objectInfoPanelPresenter.Disable();
            _goldHealthPanelPresenter.Disable();
        }

        private void OnSwapDisabled()
        {
            _towerSwapAbilityPanelPresenter.Hide();
        }

        private void OnRequestToOpenAbilityPanel(AbilityEnum playerAbility)
        {
            switch (playerAbility)
            {
                case AbilityEnum.SwapTowers:
                    _towerSwapAbilityPanelPresenter.Show();
                    break;
                
                case AbilityEnum.HealThrone:
                case AbilityEnum.TimeLapse:
                    break;

                case AbilityEnum.Unknown:
                default:
                    Debug.Log("Абилки нет.");
                    break;
            }
        }

        private void OnChooseTowerButtonClicked(GameEntity spirit)
        {
            _towerMergePanelPresenter.Activate(spirit);
            _towerMergePanelPresenter.ShowPanel();
            _chooseTowerPanelPresenter.Hide();
        }

        private void OnChooseTowerPanelEnabled()
        {
            _towerMergePanelPresenter.Hide();
        }

        private void OnTowerMergePanelClosed()
        {
            _chooseTowerPanelPresenter.MergeWindowClosed();
        }

        private void OnTowerChosen()
        {
            _chooseTowerPanelPresenter.Hide();
        }
    }
}