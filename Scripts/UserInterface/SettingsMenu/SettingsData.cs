using System;

namespace UserInterface.SettingsMenu
{
    [Serializable]
    public class SettingsData
    {
        public float MasterSlider = 0.5f;
        public float MusicSlider = 0.5f;
        public float UserInterfaceSlider = 0.5f;
        public float SoundsEffectsSlider = 0.5f;
        public float ActorUserInterfaceSlider = 0.5f;
        public float ShootingSlider = 0.5f;
        public float EnvironmentSlider = 0.5f;

        public bool IsSoundToggleActive = true;
        public bool IsMusicToggleActive = true;
    }
}