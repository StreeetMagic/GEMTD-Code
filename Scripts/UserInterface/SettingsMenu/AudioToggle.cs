using System;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.SettingsMenu
{
    public class AudioToggle : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        
        public event Action<bool> Changed;
        
        public bool ToggleValue
        {
            get => _toggle.isOn;
            set => _toggle.isOn = value;
        }

        private void OnEnable() =>
            _toggle.onValueChanged.AddListener(ChangeState);

        private void OnDisable() =>
            _toggle.onValueChanged.AddListener(ChangeState);

        private void ChangeState(bool isActive)
        {
            _toggle.isOn = isActive;
            Changed?.Invoke(isActive);
        }
    }
}