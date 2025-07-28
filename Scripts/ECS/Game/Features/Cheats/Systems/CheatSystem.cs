using Entitas;
using Game.Battle;
using Infrastructure.States.GameStates;
using Infrastructure.States.StateMachine;
using Services.StaticData;
using Services.Times;
using UnityEngine;
using UserInterface.GameplayHeadsUpDisplay;
using UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel;

namespace Game.Cheats
{
    public class CheatSystem : IExecuteSystem
    {
        private readonly GameContext _gameContext;
        private readonly IGameStateMachine _stateMachine;

        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _spirits;
        private readonly IGroup<GameEntity> _humans;
        private readonly ITimeService _timeService;
        private readonly PlayerAbilityPanelPresenter _playerAbilityPanelPresenter;
        private readonly IStaticDataService _staticDataService;
        private readonly PausePanelPresenter _pausePanelPresenter;

        public CheatSystem(
            GameContext gameContext,
            IGameStateMachine stateMachine,
            ITimeService timeService,
            PlayerAbilityPanelPresenter playerAbilityPanelPresenter,
            IStaticDataService staticDataService,
            PausePanelPresenter pausePanelPresenter
        )
        {
            _gameContext = gameContext;
            _stateMachine = stateMachine;
            _timeService = timeService;
            _playerAbilityPanelPresenter = playerAbilityPanelPresenter;
            _staticDataService = staticDataService;
            _pausePanelPresenter = pausePanelPresenter;

            _players = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.GameLoopStateEnum
                ));

            _humans = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Player,
                    GameMatcher.Human,
                    GameMatcher.GameLoopStateEnum
                ));

            _spirits = gameContext.GetGroup(GameMatcher.TowerSpirit);
        }

        public void Execute()
        {
            KillEnemies();
            ChooseFirstSpiritForEachPlayer();
            DamageThrones();
            Restart();
            TogglePause();

            UseAbilities();
            ChangeSpeed();
        }

        private void UseAbilities()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (GameEntity human in _humans)
                {
                    if (human.GameLoopStateEnum == GameLoopStateEnum.KillEnemy)
                    {
                        _playerAbilityPanelPresenter.UseAbility(AbilityEnum.SwapTowers);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                _playerAbilityPanelPresenter.UseAbility(AbilityEnum.HealThrone);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                foreach (GameEntity human in _humans)
                {
                    if (human.GameLoopStateEnum == GameLoopStateEnum.ChooseSpirit)
                    {
                        _playerAbilityPanelPresenter.UseAbility(AbilityEnum.TimeLapse);
                    }
                }
            }
        }

        private void TogglePause()
        {
            if (Input.GetKeyDown(KeyCode.Pause))
            {
                if (_timeService.IsPaused)
                {
                    return;
                    
                    _timeService.UnPause();
                    Debug.Log("▶ Resume");
                }
                else
                {
                    _timeService.Pause();
                    _pausePanelPresenter.Pause();
                    Debug.Log("⏸ Paused");
                }
            }
        }

        private void DamageThrones()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                foreach (GameEntity player in _players)
                {
                    player.ReplaceCurrentHealthPoints(player.CurrentHealthPoints - 5);
                }
            }
        }

        private void Restart()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                _stateMachine.Enter<RestartState>();
            }
        }

        private void KillEnemies()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                foreach (var enemy in _gameContext.GetEntities())
                {
                    if (enemy.isEnemy)
                    {
                        enemy.ReplaceCurrentHealthPoints(0);
                    }
                }
            }
        }

        private void ChooseFirstSpiritForEachPlayer()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                foreach (var player in _players)
                {
                    if (player.gameLoopStateEnum.Value != GameLoopStateEnum.ChooseSpirit)
                        continue;

                    player.isReadyToSwitchState = true;

                    foreach (var spirit in _spirits)
                    {
                        if (spirit.PlayerId != player.Id)
                            continue;

                        spirit.isChosen = true;
                        break;
                    }
                }
            }
        }
        
        private void ChangeSpeed()
        {
            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
                IncreaseSpeed();
            else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
                DecreaseSpeed();
        }

        private void IncreaseSpeed()
        {
            float[] multipliers = _staticDataService.ProjectConfig.TimeMultipliers;
            float current = _timeService.TimeMultiplier;
            int index = System.Array.IndexOf(multipliers, current);
            if (index < 0)
                index = 0;
            int next = Mathf.Min(index + 1, multipliers.Length - 1);
            _pausePanelPresenter.SetTimeMultiplierByInput(multipliers[next]);
        }

        private void DecreaseSpeed()
        {
            float[] multipliers = _staticDataService.ProjectConfig.TimeMultipliers;
            float current = _timeService.TimeMultiplier;
            int index = System.Array.IndexOf(multipliers, current);
            if (index < 0)
                index = 0;
            int prev = Mathf.Max(index - 1, 0);
            _pausePanelPresenter.SetTimeMultiplierByInput(multipliers[prev]);
        }
    }
}