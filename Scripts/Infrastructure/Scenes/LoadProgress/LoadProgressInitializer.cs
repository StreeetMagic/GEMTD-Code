using Infrastructure.Loading;
using Infrastructure.SceneLoading;
using UnityEngine;
using Zenject;

namespace Infrastructure.Scenes.LoadProgress
{
    public class LoadProgressInitializer : MonoBehaviour, IInitializable
    {
        [Inject]
        public void Construct(
            SceneLoader sceneLoader
        )
        {
        }

        public void Initialize()
        {
            LoadScene();
        }

        private void LoadScene()
        {
        }
    }
}