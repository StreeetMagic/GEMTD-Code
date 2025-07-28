using Services.SystemsFactoryServices;

namespace Game.Raycast
{
  public sealed class RaycastFeature : Feature
  {
    public RaycastFeature(ISystemFactory systems)
    {
      Add(systems.Create<RaycastSystem>());
    }
  }
}