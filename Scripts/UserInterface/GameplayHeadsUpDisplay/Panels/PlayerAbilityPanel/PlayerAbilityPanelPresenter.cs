using System;
using System.Collections.Generic;
using System.Globalization;
using Entitas;
using Game;
using Game.Battle;
using Game.Battle.Configs;
using Game.PlayerAbility.PlayerAbilityFactory;
using Game.Towers;
using Services.StaticData;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.GameplayHeadsUpDisplay.Buttons;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel
{
    public class PlayerAbilityPanelPresenter : Presenter<PlayerAbilityPanelView>,
        IGameLoopStateEnumListener,
        IInitializable,
        ITickable,
        IAnySwapSelectionActiveListener,
        IGoldListener
    {
        private readonly GameContext _gameContext;
        private readonly GameEntityFactories _entityFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IPlayerAbilityFactory _playerAbilityFactory;

        private IGroup<GameEntity> _uniqueRealPlayers;
        private IGroup<GameEntity> _selectedSwapPlayerAbilities;
        private IGroup<GameEntity> _abilities;

        private GameLoopStateEnum _currentGameLoopState;
        private IGroup<GameEntity> _portraits;
        private IGroup<GameEntity> _towerAbilities;

        private List<AbilitySetup> _playerAbilities;

        public event Action<AbilityEnum> RequestToOpenAbilityPanel;

        public PlayerAbilityPanelPresenter(
            PlayerAbilityPanelView view,
            IStaticDataService staticDataService,
            GameContext gameContext,
            GameEntityFactories entityFactory,
            IPlayerAbilityFactory playerAbilityFactory
        ) : base(view)
        {
            _staticDataService = staticDataService;
            _gameContext = gameContext;
            _entityFactory = entityFactory;
            _playerAbilityFactory = playerAbilityFactory;
        }

        public void Initialize()
        {
            _uniqueRealPlayers = _gameContext.GetGroup(
                GameMatcher
                    .AllOf(
                        GameMatcher.Human,
                        GameMatcher.Player,
                        GameMatcher.Id));

            _selectedSwapPlayerAbilities = _gameContext.GetGroup(GameMatcher
                .AllOf(GameMatcher.SwapSelectionActive
                ));

            _abilities = _gameContext.GetGroup(GameMatcher
                .AllOf(GameMatcher.AbilityEnum));

            _towerAbilities = _gameContext.GetGroup(GameMatcher
                .AllOf(GameMatcher.Tower,
                    GameMatcher.Abilities,
                    GameMatcher.AbilityEnum));

            _portraits = _gameContext.GetGroup(GameMatcher.PortraitTarget);

            _playerAbilities = _staticDataService.ProjectConfig.PlayerConfig.PlayerAbilitySetup;
        }

        public void Enable()
        {
            GetRealPlayer().AddAnySwapSelectionActiveListener(this);
            GetRealPlayer().AddGoldListener(this);

            Show();

            foreach (GameEntity player in _uniqueRealPlayers)
                player.AddGameLoopStateEnumListener(this);

            RegisterButtonsForPlayer();

            foreach (var portrait in _portraits)
            {
                RegistrarAbilityView(_playerAbilities, true);

                if (portrait.isHuman)
                    ValidatePlayerAbilityButton(_currentGameLoopState);
            }
        }

        private void RegisterButtonsForPlayer()
        {
            for (int i = 0; i < View.ButtonsView.Length; i++)
            {
                var i1 = i;

                View.ButtonsView[i].Button.onClick.AddListener(() =>
                {
                    AbilityEnum abilityEnum = View.ButtonsView[i1].PlayerAbilityEnum;

                    foreach (var playerAbility in _abilities)
                    {
                        if (playerAbility.AbilityEnum == abilityEnum)
                        {
                            if (playerAbility.isCooldownUp)
                            {
                                UseAbility(abilityEnum);

                                return;
                            }
                        }
                    }
                });
            }
        }

        public void Disable()
        {
            Hide();
            GetRealPlayer()?.RemoveAnySwapSelectionActiveListener(this);
            GetRealPlayer()?.RemoveGoldListener(this);

            foreach (GameEntity player in _uniqueRealPlayers)
                player.RemoveGameLoopStateEnumListener(this);

            for (int i = 0; i < View.ButtonsView.Length; i++)
                View.ButtonsView[i].Button.onClick.RemoveAllListeners();
        }

        private void RegistrarAbilityView(List<AbilitySetup> abilities, bool isPlayerAbility)
        {
            for (int i = 0; i < View.ButtonsView.Length; i++)
            {
                AbilityButtonView button = View.ButtonsView[i];

                if (i > abilities.Count - 1)
                {
                    button.Button.gameObject.SetActive(false);
                }
                else
                {
                    button.Button.gameObject.SetActive(true);
                    AbilitySetup ability = abilities[i];

                    button.Cooldown.text = ability.Cooldown.ToString(CultureInfo.InvariantCulture);
                    button.Cost.text = ability.Cost.ToString(CultureInfo.InvariantCulture);
                    //   button.Level.text = ability.Level.ToString(CultureInfo.InvariantCulture);
                    button.Icon.sprite = ability.Icon;
                    button.NameText.text = ability.Name;
                    button.PlayerAbilityEnum = ability.AbilityEnum;

                    if (isPlayerAbility && button.PlayerAbilityEnum == AbilityEnum.SwapTowers)
                        ActivateOutlineAbility(button);
                    else
                        DeactivateOutline(button);
                }
            }
        }

        public void UseAbility(AbilityEnum playerAbilityAbility)
        {
            GameEntity realPlayer = GetRealPlayer();

            if (realPlayer == null)
                throw new NullReferenceException();

            RequestToOpenAbilityPanel?.Invoke(playerAbilityAbility);

            switch (playerAbilityAbility)
            {
                case AbilityEnum.SwapTowers:
                    _playerAbilityFactory.CreateSwapRequest(realPlayer.Id);
                    _entityFactory.CreateCancelSelectionRequest();
                    break;
                case AbilityEnum.HealThrone:
                    _playerAbilityFactory.CreateHealThroneRequest(realPlayer.Id);
                    break;

                case AbilityEnum.TimeLapse:
                    _playerAbilityFactory.CreateTimeLapseRequest(realPlayer.Id);
                    break;

                case AbilityEnum.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerAbilityAbility), playerAbilityAbility, null);
            }

            ValidatePlayerAbilityButton(_currentGameLoopState);

            foreach (var portrait in _portraits)
            {
            }
        }

        private GameEntity GetRealPlayer()
        {
            GameEntity[] allPlayers = _uniqueRealPlayers.GetEntities();

            if (allPlayers.Length == 0)
                return null;

            return allPlayers[0];
        }

        public void OnGameLoopStateEnum(GameEntity entity, GameLoopStateEnum value)
        {
            _currentGameLoopState = value;

            ValidatePlayerAbilityButton(_currentGameLoopState);
        }

        public void Tick()
        {
            foreach (GameEntity portrait in _portraits)
            {
                if (portrait.hasTowerEnum)
                {
                    TowerConfig towerConfig = _staticDataService.GetTowerConfig(portrait.TowerEnum);
                    List<AbilitySetup> abilities = towerConfig.Abilities;

                    RegistrarAbilityView(abilities, false);
                }
                else if (portrait.isEnemy)
                {
                    foreach (AbilityButtonView button in View.ButtonsView)
                    {
                        button.Button.gameObject.SetActive(false);
                    }
                }
                else
                {
                    RegistrarAbilityView(_playerAbilities, true);
                    UpdateCooldownVisual();
                }
            }
        }

        public void OnGold(GameEntity entity, int value)
        {
            foreach (var portrait in _portraits)
            {
                if (portrait.isHuman)
                    ValidatePlayerAbilityButton(_currentGameLoopState);
            }
        }

        public void DeactivateOutline(AbilityEnum swapTowers)
        {
            foreach (AbilityButtonView button in View.ButtonsView)
            {
                if (button.PlayerAbilityEnum == swapTowers)
                    button.OutlineActiveAbility.gameObject.SetActive(false);
            }
        }

        public void OnAnySwapSelectionActive(GameEntity entity)
        {
            foreach (AbilityButtonView abilityButtonView in View.ButtonsView)
            {
                switch (abilityButtonView.PlayerAbilityEnum)
                {
                    case AbilityEnum.SwapTowers:
                        ActivateOutlineAbility(abilityButtonView);
                        break;
                }
            }
        }

        private void ActivateOutlineAbility(AbilityButtonView button)
        {
            bool hasSwapAbility = _selectedSwapPlayerAbilities.count == 0;

            button.OutlineActiveAbility.gameObject.SetActive(!hasSwapAbility);
        }

        private void DeactivateOutline(AbilityButtonView button)
        {
            button.OutlineActiveAbility.gameObject.SetActive(false);
        }

        private void ValidatePlayerAbilityButton(GameLoopStateEnum value)
        {
            GameEntity realPlayer = GetRealPlayer();

            foreach (AbilityButtonView button in View.ButtonsView)
            {
                if (button.PlayerAbilityEnum == AbilityEnum.Unknown)
                    return;
                
                if (button.PlayerAbilityEnum == AbilityEnum.SwapTowers || button.PlayerAbilityEnum == AbilityEnum.TimeLapse || button.PlayerAbilityEnum == AbilityEnum.HealThrone)

                {
                    // if (!button.gameObject.activeSelf)
                    //     continue;

                    if (GameLoopStateEnum.PlaceSpirit == value || GameLoopStateEnum.ChooseSpirit == value ||
                        GameLoopStateEnum.SpiritToTower == value)
                        DeactivateOutline(AbilityEnum.SwapTowers);

                    AbilitySetup abilitySetup = _staticDataService.GetPlayerAbilitySetup(button.PlayerAbilityEnum);

                    bool isAllowedInCurrentState = false;

                    foreach (GameLoopStateEnum state in abilitySetup.CanUseIn)
                    {
                        if (value == state)
                        {
                            isAllowedInCurrentState = true;

                            break;
                        }
                    }

                    if (!isAllowedInCurrentState)
                    {
                        button.Button.interactable = false;

                        continue;
                    }

                    if (realPlayer != null && realPlayer.hasGold)
                        button.Button.interactable = realPlayer.Gold >= abilitySetup.Cost;
                    else
                        button.Button.interactable = false;
                }
            }
        }

        private void UpdateButtonSlider(GameEntity abilityEntity, Image slider)
        {
            float maxCd = _staticDataService.GetPlayerAbilitySetup(abilityEntity.AbilityEnum).Cooldown;

            if (abilityEntity.isCooldownUp)
            {
                slider.fillAmount = 0;

                return;
            }

            if (!abilityEntity.hasCooldownLeft)
            {
                slider.fillAmount = 0;

                return;
            }

            float sliderValue = Mathf.Clamp01(abilityEntity.CooldownLeft / maxCd);

            slider.fillAmount = sliderValue;
        }

        private void UpdateCooldownVisual()
        {
            foreach (var button in View.ButtonsView)
            {
                foreach (var playerAbility in _abilities)
                {
                    if (playerAbility.AbilityEnum == button.PlayerAbilityEnum)
                        UpdateButtonSlider(playerAbility, button.CooldownSlider);
                }
            }
        }
    }
}