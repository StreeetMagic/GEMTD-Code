using Services.SystemsFactoryServices;

namespace Game.Lifetime
{
  public sealed class DeathFeature : Feature
  {
    public DeathFeature(ISystemFactory systems)
    {
      Add(systems.Create<MarkDeadSystem>());
      Add(systems.Create<UnapplyStatusesOfDeadTargetSystem>());
    }
  }
}