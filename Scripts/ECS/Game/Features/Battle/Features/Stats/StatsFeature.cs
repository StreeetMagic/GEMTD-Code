using Services.SystemsFactoryServices;

namespace Game.Battle
{
  public sealed class StatsFeature : Feature
  {
    public StatsFeature(ISystemFactory systems)
    {
      Add(systems.Create<StatChangeSystem>());
      
      Add(systems.Create<ApplyMoveSpeedFromStatsSystem>());
      Add(systems.Create<ApplyAttackSpeedFromStatsSystem>());
      Add(systems.Create<ApplyArmorFromStatsSystem>());
      Add(systems.Create<ApplyAdditionalProjectilesFromStatsSystem>());
    }
  }
}