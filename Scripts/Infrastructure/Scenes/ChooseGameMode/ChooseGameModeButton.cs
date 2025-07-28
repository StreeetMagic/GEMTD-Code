using Infrastructure.Loading;
using Infrastructure.SceneLoading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Infrastructure.Scenes.ChooseGameMode
{
    public class ChooseGameModeButton : MonoBehaviour
    {
        [Inject]
        private SceneLoader _sceneLoader;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                //     _sceneLoader.Load(SceneId.LoadConfigs);
            });
        }
    }
}