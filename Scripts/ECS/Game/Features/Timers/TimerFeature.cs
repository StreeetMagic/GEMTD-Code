using Services.SystemsFactoryServices;

namespace Game.Timers
{
  public sealed class TimerFeature : Feature
  {
    public TimerFeature(ISystemFactory systems)
    {
      Add(systems.Create<TimerSystem>());
    }
  }
}