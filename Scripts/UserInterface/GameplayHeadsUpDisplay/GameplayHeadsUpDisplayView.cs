using System;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;

namespace UserInterface.GameplayHeadsUpDisplay
{
    public class GameplayHeadsUpDisplayView : View
    {
        public RectTransform rectTransform;
        
        [Required]
        public Button SettingButton;
        
        public event Action SettingButtonPressed;

        private void Awake()
        {
            SettingButton.onClick.AddListener(() => SettingButtonPressed?.Invoke());
        }

        private void OnDestroy()
        {
            SettingButton.onClick.RemoveAllListeners();
        }
    }
}