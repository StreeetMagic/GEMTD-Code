using Game.Battle;
using Game.Battle.Factory;
using Game.Binder;
using Game.EntityIndices;
using Game.Factory;
using Game.PlayerAbility.PlayerAbilityFactory;
using Game.Towers;
using Infrastructure.Loading;
using Infrastructure.SceneLoading;
using Infrastructure.Scenes.Hubs;
using Infrastructure.States.Factory;
using Infrastructure.States.GameStates;
using Infrastructure.States.StateMachine;
using Services.AssetProviders;
using Services.AudioServices;
using Services.Cameras;
using Services.Collisions;
using Services.CursorServices;
using Services.Identifiers;
using Services.MazeBuilders;
using Services.PersistentProgresses;
using Services.Physics;
using Services.ProjectData;
using Services.Randoms;
using Services.SaveLoadServices;
using Services.StaticData;
using Services.SystemsFactoryServices;
using Services.Times;
using Services.TowerRandomers;
using Services.ViewContainerProviders;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UserInterface;
using UserInterface.GameplayHeadsUpDisplay;
using UserInterface.GameplayHeadsUpDisplay.CheatsPanel;
using UserInterface.GameplayHeadsUpDisplay.FinishPanel;
using UserInterface.GameplayHeadsUpDisplay.GoldHealthPanels;
using UserInterface.GameplayHeadsUpDisplay.InfoPanel;
using UserInterface.GameplayHeadsUpDisplay.ObjectInfoPanel;
using UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel;
using UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel.SwapAbility;
using UserInterface.GameplayHeadsUpDisplay.PlayerPanel;
using UserInterface.GameplayHeadsUpDisplay.TimerPanel;
using UserInterface.MainMenu;
using UserInterface.MazeSelectorMenu;
using UserInterface.SettingsMenu;
using Zenject;

namespace Infrastructure.Projects
{
    public class ProjectInstaller : MonoInstaller
    {
        [Required] public AudioMixer AudioMixer;
        [Required] public GameplayHeadsUpDisplayView GameplayHeadsUpDisplayView;
        [Required] public SettingsMenuView SettingsMenuView;
        [Required] public MainMenuView MainMenuView;
        [Required] public MazeSelectorView MazeSelectorView;
        [Required] public ChooseTowerPanelView ChooseTowerPanelView; 
        [Required] public TowerMergePanelView TowerMergePanelView;
        [Required] public TimerPanelView TimerPanelView;
        [Required] public PlayerPanelView PlayerPanelView;
        [Required] public InfoPanelView InfoPanelView;
        [Required] public CurtainView CurtainView;
        [Required] public PlayerAbilityPanelView PlayerAbilityPanelView;
        [Required] public CheatsPanelView CheatsPanelView;
        [Required] public AudioSourceContainer AudioSourceContainer;
        [Required] public TowerSwapAbilityPanelView TowerSwapAbilityView;
        [Required] public PausePanelView PausePanelView;
        [Required] public FinishPanelView FinishPanelView;
        [Required] public AudioListener AudioListener;
        [Required] public ObjectInfoPanelView ObjectInfoPanelView;
        [Required] public GoldHealthPanelView GoldHealthPanelView;

        public override void InstallBindings()
        {
            Container.Bind<Contexts>().FromInstance(Contexts.sharedInstance).AsSingle();
            Container.Bind<GameContext>().FromInstance(Contexts.sharedInstance.game).AsSingle();

            BindServices();

            Container.Bind<IIdentifierService>().To<IdentifierService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectInitializer>().FromInstance(GetComponent<ProjectInitializer>()).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameEntityViewSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameEntityViewFactory>().AsSingle();

            Container.Bind<ICollisionRegistry>().To<CollisionRegistry>().AsSingle();
            Container.Bind<IPhysicsService>().To<PhysicsService>().AsSingle();

            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerPrefsSaveLoad>().AsSingle();
            Container.BindInterfacesAndSelfTo<PersistentProgressService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AudioService>().AsSingle().NonLazy();

            Container.Bind<IAssetProvider>().To<ResourceFolderAssetProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<CursorService>().AsSingle();

            Container.Bind<AudioMixer>().FromInstance(AudioMixer).AsSingle();
            Container.Bind<ITimeService>().To<UnityTimeService>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SystemFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MazeBuilder>().AsSingle();
            Container.BindInterfacesAndSelfTo<TowerRandomer>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityRandomService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ViewContainerProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectDataService>().AsSingle();

            UserInterface();

            States();
            GameFactories();
            BindEntityIndices();

            Container.Bind<GameplayInitializer>().FromInstance(GetComponent<GameplayInitializer>()).AsSingle().NonLazy();
            Container.Bind<AudioSourceContainer>().FromInstance(AudioSourceContainer).AsSingle();
            Container.BindInterfacesAndSelfTo<AudioListener>().FromInstance(AudioListener).AsSingle();
        }

        private void UserInterface()
        {
            Container.BindInterfacesAndSelfTo<SettingsMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<SettingsMenuView>().FromInstance(SettingsMenuView).AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameplayHeadsUpDisplayPresenter>().AsSingle().NonLazy();
            Container.Bind<GameplayHeadsUpDisplayView>().FromInstance(GameplayHeadsUpDisplayView).AsSingle();

            Container.BindInterfacesAndSelfTo<MainMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<MainMenuView>().FromInstance(MainMenuView).AsSingle();

            Container.BindInterfacesAndSelfTo<MazeSelectorPresenter>().AsSingle().NonLazy();
            Container.Bind<MazeSelectorView>().FromInstance(MazeSelectorView).AsSingle();

            Container.BindInterfacesAndSelfTo<ChooseTowerPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<ChooseTowerPanelView>().FromInstance(ChooseTowerPanelView).AsSingle();

            Container.BindInterfacesAndSelfTo<TowerMergePanelPresenter>().AsSingle().NonLazy();
            Container.Bind<TowerMergePanelView>().FromInstance(TowerMergePanelView).AsSingle();

            Container.BindInterfacesAndSelfTo<TimerPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<TimerPanelView>().FromInstance(TimerPanelView).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<PlayerPanelView>().FromInstance(PlayerPanelView).AsSingle();

            Container.BindInterfacesAndSelfTo<InfoPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<InfoPanelView>().FromInstance(InfoPanelView).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerAbilityPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<PlayerAbilityPanelView>().FromInstance(PlayerAbilityPanelView).AsSingle();

            Container.BindInterfacesAndSelfTo<CheatsPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<CheatsPanelView>().FromInstance(CheatsPanelView).AsSingle();

            Container.BindInterfacesAndSelfTo<CurtainPresenter>().AsSingle().NonLazy();
            Container.Bind<CurtainView>().FromInstance(CurtainView).AsSingle();

            Container.BindInterfacesAndSelfTo<TowerSwapAbilityPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<TowerSwapAbilityPanelView>().FromInstance(TowerSwapAbilityView).AsSingle();

            Container.BindInterfacesAndSelfTo<PausePanelPresenter>().AsSingle().NonLazy();
            Container.Bind<PausePanelView>().FromInstance(PausePanelView).AsSingle();
            
            Container.BindInterfacesAndSelfTo<FinishPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<FinishPanelView>().FromInstance(FinishPanelView).AsSingle();
            
            Container.BindInterfacesAndSelfTo<ObjectInfoPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<ObjectInfoPanelView>().FromInstance(ObjectInfoPanelView).AsSingle();
            
            Container.BindInterfacesAndSelfTo<GoldHealthPanelPresenter>().AsSingle().NonLazy();
            Container.Bind<GoldHealthPanelView>().FromInstance(GoldHealthPanelView).AsSingle();
        }

        private void States()
        {
            Container.BindInterfacesAndSelfTo<BootstrapState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadProgressState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingHomeScreenState>().AsSingle();
            Container.BindInterfacesAndSelfTo<HomeScreenState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingBattleState>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleEnterState>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleLoopState>().AsSingle();
            Container.BindInterfacesAndSelfTo<RestartState>().AsSingle();
            Container.BindInterfacesAndSelfTo<MazeSelectorState>().AsSingle();
        }

        private void GameFactories()
        {
            Container.BindInterfacesAndSelfTo<GameEntityFactories>().AsSingle();
            Container.BindInterfacesAndSelfTo<VisualEffectFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<AbilityFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpiritFactory>().AsSingle();
            Container.Bind<IArmamentFactory>().To<ArmamentFactory>().AsSingle();
            Container.Bind<IEffectFactory>().To<EffectFactory>().AsSingle();
            Container.Bind<IStatusFactory>().To<StatusFactory>().AsSingle();
            Container.Bind<IPlayerAbilityFactory>().To<PlayerAbilityFactory>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<IStatusApplier>().To<StatusApplier>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraProvider>().AsSingle();
        }

        private void BindEntityIndices()
        {
            Container.BindInterfacesAndSelfTo<GameEntityIndices>().AsSingle();
        }

        private void OnDestroy()
        {
            Container.Resolve<SettingsMenuPresenter>().Dispose();
        }
    }
}