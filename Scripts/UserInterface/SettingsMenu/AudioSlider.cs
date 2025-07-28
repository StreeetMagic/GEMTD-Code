using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UserInterface.SettingsMenu
{
    public class AudioSlider : MonoBehaviour
    {
        private const float Multiplier = 20;
        private const float MinVolume = 0.0001f;
        private const float MaxValue = 1;

        [Required] [SerializeField] private AudioMixerGroup _mixer;

        [Required] [SerializeField] private Slider _slider;

        [Required] [SerializeField] private TextMeshProUGUI _text;

        [Required] [SerializeField] private AudioToggle _audioToggle;

        public float SliderValue
        {
            get { return _slider.value; }
            set
            {
                if (value < MinVolume)
                    value = MinVolume;

                if (value > MaxValue)
                    value = MaxValue;

                _slider.value = value;

                Initialize(value);
            }
        }

        private void Initialize(float volume)
        {
            float volumeValue = (float)Math.Log10(volume) * Multiplier;
            
            _mixer.audioMixer.SetFloat(_mixer.name, volumeValue);

            if (_audioToggle)
                if (_audioToggle.ToggleValue == false)
                {
                    _mixer.audioMixer.SetFloat(_mixer.name, -80);
                }
            
            if (_text)
                _text.text = $"{(int)(volume * 100)}";
        }

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(ChangeVolume);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(ChangeVolume);
        }

        private void ChangeVolume(float volume)
        {
            if (volume < MinVolume)
                volume = MinVolume;

            if (_text)
                _text.text = $"{(int)(volume * 100)}";

            if (_audioToggle)
                if (_audioToggle.ToggleValue == false)
                    _audioToggle.ToggleValue = true;

            float volumeValue = (float)Math.Log10(volume) * Multiplier;

            _mixer.audioMixer.SetFloat(_mixer.name, volumeValue);
        }
    }
}