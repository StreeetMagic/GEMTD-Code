using Game.Battle;
using Game.Cameras;
using Game.Cheats;
using Game.Cursor;
using Game.Destruct;
using Game.Enemies;
using Game.Highlight;
using Game.Inputs;
using Game.Lifetime;
using Game.PlayerAbility;
using Game.PortraitCameras;
using Game.Raycast;
using Game.Selection;
using Game.Timers;
using Game.Towers;
using Services.SystemsFactoryServices;

namespace Game.GameMainFeature
{
    public sealed class GameplayFeature : Feature
    {
        public GameplayFeature(ISystemFactory systems)
        {
            Add(systems.Create<InitializeGameLoopSystem>());
            Add(systems.Create<CameraFeature>());
            Add(systems.Create<PortraitCameraFeature>());
            Add(systems.Create<SpawnEnemySystem>());
            Add(systems.Create<ChangeRoundSystem>());
            Add(systems.Create<SetLevelByRoundSystem>());
            Add(systems.Create<UpdateTotalGameTimeSystem>());

            Add(systems.Create<GameEventSystems>());

            Add(systems.Create<InputFeature>());
            Add(systems.Create<BindViewFeature>());
            Add(systems.Create<TimerFeature>());
            Add(systems.Create<CursorFeature>());
            Add(systems.Create<CheatFeature>());

            Add(systems.Create<RaycastFeature>());
            Add(systems.Create<HighlightFeature>());
            Add(systems.Create<SelectionFeature>());

            Add(systems.Create<BattleFeature>());
            Add(systems.Create<TowerFeature>());
            Add(systems.Create<DeathFeature>());
            Add(systems.Create<EnemyFeature>());
            Add(systems.Create<PlayerFeature>());
            Add(systems.Create<ProcessDestructedFeature>());
            Add(systems.Create<PlayerAbilityFeature>());
        }
    }
}