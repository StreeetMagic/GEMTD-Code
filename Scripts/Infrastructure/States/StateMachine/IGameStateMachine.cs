using System;
using Infrastructure.States.StateInfrastructure;

namespace Infrastructure.States.StateMachine
{
  public interface IGameStateMachine 
  {
    void Enter<TState>() where TState : class, IState;
    void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;
    IExitableState ActiveState { get; }
  }
}