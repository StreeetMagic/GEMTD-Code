using DG.Tweening;
using Infrastructure.Loading;
using Infrastructure.SceneLoading;
using UnityEngine;
using Zenject;

namespace Infrastructure.Scenes.Hubs
{
    public class GameplayInitializer : MonoBehaviour
    {
        [Inject]
        public void Construct(
            SceneLoader sceneLoader,
            GameContext gameContext
        )
        {
        }

        private void Start()
        {
            Time.timeScale = 1f;

           // StartCoroutine(SetBlocksParents());
        }

        public void Restart()
        {
            Destroy();

           // _sceneLoader.Load(SceneId.LoadProgress);
        }

        private void Destroy()
        {
            DOTween.KillAll();
            Time.timeScale = 0f;
        }
    }
}