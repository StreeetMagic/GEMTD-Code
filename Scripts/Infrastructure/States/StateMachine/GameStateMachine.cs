using System;
using Infrastructure.States.Factory;
using Infrastructure.States.StateInfrastructure;
using UnityEngine;
using Zenject;

namespace Infrastructure.States.StateMachine
{
    public class GameStateMachine : IGameStateMachine, ITickable
    {
        private readonly IStateFactory _stateFactory;

        public GameStateMachine(IStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public IExitableState ActiveState { get; private set; }

        public void Tick()
        {
            if (ActiveState is IUpdateable updateableState)
                updateableState.Update();
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            ActiveState?.Exit();

            TState state = _stateFactory.GetState<TState>();
            ActiveState = state;

            return state;
        }
    }
}