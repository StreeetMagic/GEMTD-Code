using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UserInterface.SettingsMenu
{
    public class SettingsMenuView : View, IPointerClickHandler
    {
        [Required]
        public Button ExitButton;
    
        [Header("Sliders")]
        [Required]
        public AudioSlider MasterSlider;
        [Required]
        public AudioSlider MusicSlider;
        [Required]
        public AudioSlider UserInterfaceSlider;
        [Required]
        public AudioSlider SoundsEffectsSlider;
        [Required]
        public AudioSlider ActorUserInterfaceSlider;
        [Required]
        public AudioSlider ShootingSlider;
        [Required]
        public AudioSlider EnvironmentSlider;

        [Header("Toggles")]
        [Required]
        public AudioToggle MusicToggle;

        [Required]
        public AudioToggle SoundToggle;

        public event Action ExitButtonPressed;
        public event Action<bool> MusicToggleChanged;
        public event Action<bool> SoundToggleChanged;

        public event Action ClickedOutside;

        private void Awake()
        {
            ExitButton.onClick.AddListener(() => ExitButtonPressed?.Invoke());
        }

        private void OnDestroy()
        {
            ExitButton.onClick.RemoveAllListeners();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    transform as RectTransform, 
                    eventData.position, 
                    eventData.pressEventCamera))
            {
                ClickedOutside?.Invoke();
            }
        }
        
        private void OnEnable()
        {
            MusicToggle.Changed +=  OnMusicToggleChanged;
            SoundToggle.Changed +=  OnSoundToggleChanged;
        }
        
        public void OnDisable()
        {
            MusicToggle.Changed -=  OnMusicToggleChanged;
            SoundToggle.Changed -=  OnSoundToggleChanged;
        }

        private void OnSoundToggleChanged(bool isActive) => 
            SoundToggleChanged?.Invoke(isActive);

        private void OnMusicToggleChanged(bool isActive) => 
            MusicToggleChanged?.Invoke(isActive);
    }
}