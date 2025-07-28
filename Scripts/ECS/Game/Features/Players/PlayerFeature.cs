using Game.ChooseSpirit;
using Game.GameFinish;
using Game.KillEnemy;
using Game.PlaceSpirit;
using Game.SpiritToTower;
using Game.Towers.MergeSpirits.Systems;
using Services.SystemsFactoryServices;

namespace Game
{
    public sealed class PlayerFeature : Feature
    {
        public PlayerFeature(ISystemFactory systems)
        {
            Add(systems.Create<ChooseSpiritStateSystem>());
            Add(systems.Create<KillEnemyStateSystem>());
            Add(systems.Create<PlaceSpiritStateSystem>());
            Add(systems.Create<SpiritToTowerStateSystem>());
            Add(systems.Create<RoundTimerResetAndStartSystem>());
            Add(systems.Create<RoundTimerTickSystem>());
            Add(systems.Create<RoundTimerStartSystem>());
            Add(systems.Create<FreezeRoundTimerOnRoundCompleteSystem>());
            Add(systems.Create<MarkMergeableSpiritsSystem>());
            Add(systems.Create<PlayerDefeatSystem>());
        }
    }
}