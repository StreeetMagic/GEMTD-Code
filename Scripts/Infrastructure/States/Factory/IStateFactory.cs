using Infrastructure.States.StateInfrastructure;

namespace Infrastructure.States.Factory
{
  public interface IStateFactory
  {
    T GetState<T>() where T : class, IExitableState;
  }
}