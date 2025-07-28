using Infrastructure.Loading;
using Infrastructure.SceneLoading;
using Infrastructure.States.GameStates;
using Infrastructure.States.StateMachine;
using Services.CoroutineRunners;
using UnityEngine;
using Zenject;

namespace Infrastructure.Projects
{
    public class ProjectInitializer : MonoBehaviour, IInitializable, ICoroutineRunner
    {
        private IGameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(ISceneLoader sceneLoader, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Initialize()
        {
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}