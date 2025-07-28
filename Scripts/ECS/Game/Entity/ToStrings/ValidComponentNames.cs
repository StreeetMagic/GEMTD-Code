using Game.Battle;
using Game.Cameras;
using Game.Cursor;
using Game.Enemies;
using Game.GameMainFeature;
using Game.Inputs;
using Game.KillEnemy;
using Game.Maze;
using Game.PortraitCameras;
using Game.PortraitCameras.Comps;
using Game.Towers;

namespace Game.Entity.ToStrings
{
    public static class ValidComponentNames
    {
        public static string[] Value =
        {
            nameof(CursorComponent),
            nameof(CameraComponent),
            nameof(BlockComponent),
            nameof(UserInputComponent),
            nameof(CheckPointComponent),
            nameof(StartPointComponent),
            nameof(FinishPointComponent),
            nameof(TowerSpiritComponent),
            nameof(TowerComponent),
            nameof(WallComponent),
            nameof(ArmamentComponent),
            nameof(EnemyComponent),
            nameof(AbilityEnumComponent),
            nameof(PlayerComponent),
            nameof(GameMainComponent),
            nameof(RoundCompleteComponent),
            nameof(EnemyFragComponent),
            nameof(StatusComponent),
            nameof(PortraitCameraComponent),
        };
    }
}