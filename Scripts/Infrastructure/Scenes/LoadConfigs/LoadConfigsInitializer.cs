using Infrastructure.Loading;
using Infrastructure.SceneLoading;
using Services.AudioServices;
using UnityEngine;
using Zenject;

namespace Infrastructure.Scenes.LoadConfigs
{
    public class LoadConfigsInitializer : MonoBehaviour, IInitializable
    {
        private AudioService _audioService;

        [Inject]
        public void Construct(
            SceneLoader sceneLoader,
            AudioService audioService
        )
        {
            _audioService = audioService;
        }

        public void Initialize()
        {
            //_audioService.Init();
            //      _sceneLoader.Load(SceneId.LoadProgress);
        }
    }
}