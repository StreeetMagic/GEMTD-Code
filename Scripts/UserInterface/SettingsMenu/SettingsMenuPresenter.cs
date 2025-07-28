using System;
using Services.AudioServices.AudioMixers;
using Services.PersistentProgresses;
using Services.SaveLoadServices;
using Services.StaticData;
using Services.Times;
using UnityEngine.Audio;
using UserInterface.GameplayHeadsUpDisplay;
using UserInterface.MainMenu;
using UserInterface.MazeSelectorMenu;

namespace UserInterface.SettingsMenu
{
    public class SettingsMenuPresenter :
        Presenter<SettingsMenuView>, 
        IDisposable, 
        IProgressWriter
    {
        private readonly GameplayHeadsUpDisplayPresenter _gameplayHeadsUpDisplayPresenter;
        private readonly MazeSelectorPresenter _mazeSelectorPresenter;
        private readonly MainMenuPresenter _mainMenuPresenter;
        private readonly ITimeService _timeService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly AudioMixer _audioMixer;
        private readonly IStaticDataService _staticDataService;
        private readonly GameContext _gameContext;

        public SettingsMenuPresenter(
            SettingsMenuView view,
            GameplayHeadsUpDisplayPresenter gameplayHeadsUpDisplayPresenter,
            MazeSelectorPresenter mazeSelectorPresenter,
            MainMenuPresenter mainMenuPresenter,
            ITimeService timeService,
            ISaveLoadService saveLoadService,
            AudioMixer audioMixer,
            IStaticDataService staticDataService
        ) : base(view)
        {
            _gameplayHeadsUpDisplayPresenter = gameplayHeadsUpDisplayPresenter;
            _mazeSelectorPresenter = mazeSelectorPresenter;
            _mainMenuPresenter = mainMenuPresenter;
            _timeService = timeService;
            _saveLoadService = saveLoadService;
            _audioMixer = audioMixer;
            _staticDataService = staticDataService;
            _mainMenuPresenter.SettingPanelActivated += OnSettingPanelActivated;
            _gameplayHeadsUpDisplayPresenter.SettingPanelActivated += OnSettingPanelActivated;
            _mazeSelectorPresenter.SettingPanelActivated += OnSettingPanelActivated;
        }

        private void OnSettingPanelActivated()
        {
            View.ExitButtonPressed += OnExitButtonPressed;
            View.MusicToggleChanged += OnMusicToggleChanged;
            View.SoundToggleChanged += OnSoundToggleChanged;

            Show();
            _timeService.Pause();
        }

        private void OnExitButtonPressed()
        {
            _saveLoadService.SaveProgress();
            View.ExitButtonPressed -= OnExitButtonPressed;
            View.ClickedOutside -= OnExitButtonPressed;
            _timeService.UnPause();
            Hide();
        }

        public void Dispose()
        {
            _mainMenuPresenter.SettingPanelActivated -= OnSettingPanelActivated;
            _gameplayHeadsUpDisplayPresenter.SettingPanelActivated -= OnSettingPanelActivated;
            _mazeSelectorPresenter.SettingPanelActivated -= OnSettingPanelActivated;
            View.MusicToggleChanged -= OnMusicToggleChanged;
            View.SoundToggleChanged -= OnSoundToggleChanged;
        }

        public void ReadProgress(ProjectProgress projectProgress)
        {
            View.SoundToggle.ToggleValue = projectProgress.SettingsData.IsSoundToggleActive;
            View.MusicToggle.ToggleValue = projectProgress.SettingsData.IsMusicToggleActive;

            View.MasterSlider.SliderValue = projectProgress.SettingsData.MasterSlider;
            View.MusicSlider.SliderValue = projectProgress.SettingsData.MusicSlider;
            View.UserInterfaceSlider.SliderValue = projectProgress.SettingsData.UserInterfaceSlider;
            View.SoundsEffectsSlider.SliderValue = projectProgress.SettingsData.SoundsEffectsSlider;
            View.ActorUserInterfaceSlider.SliderValue = projectProgress.SettingsData.ActorUserInterfaceSlider;
            View.ShootingSlider.SliderValue = projectProgress.SettingsData.ShootingSlider;
            View.EnvironmentSlider.SliderValue = projectProgress.SettingsData.EnvironmentSlider;
        }

        public void WriteProgress(ProjectProgress projectProgress)
        {
            projectProgress.SettingsData.MasterSlider = View.MasterSlider.SliderValue;
            projectProgress.SettingsData.MusicSlider = View.MusicSlider.SliderValue;
            projectProgress.SettingsData.UserInterfaceSlider = View.UserInterfaceSlider.SliderValue;
            projectProgress.SettingsData.SoundsEffectsSlider = View.SoundsEffectsSlider.SliderValue;
            projectProgress.SettingsData.ActorUserInterfaceSlider = View.ActorUserInterfaceSlider.SliderValue;
            projectProgress.SettingsData.ShootingSlider = View.ShootingSlider.SliderValue;
            projectProgress.SettingsData.EnvironmentSlider = View.EnvironmentSlider.SliderValue;
            
            projectProgress.SettingsData.IsSoundToggleActive = View.SoundToggle.ToggleValue;
            projectProgress.SettingsData.IsMusicToggleActive = View.MusicToggle.ToggleValue;
        }

        private void OnSoundToggleChanged(bool isActive)
        {
            if (isActive)
            {
                View.UserInterfaceSlider.SliderValue = View.UserInterfaceSlider.SliderValue;
                View.SoundsEffectsSlider.SliderValue = View.SoundsEffectsSlider.SliderValue;
                View.ActorUserInterfaceSlider.SliderValue = View.ActorUserInterfaceSlider.SliderValue;
                View.ShootingSlider.SliderValue = View.ShootingSlider.SliderValue;
                View.EnvironmentSlider.SliderValue = View.EnvironmentSlider.SliderValue;
            }
            else
            {
                ChangeAudioMixerVolume(AudioMixerGroupEnum.UserInterface);
                ChangeAudioMixerVolume(AudioMixerGroupEnum.SoundEffects);
                ChangeAudioMixerVolume(AudioMixerGroupEnum.ActorUserInterface);
                ChangeAudioMixerVolume(AudioMixerGroupEnum.Shooting);
                ChangeAudioMixerVolume(AudioMixerGroupEnum.Environment);
            }
        }

        private void OnMusicToggleChanged(bool isActive)
        {
            if (isActive)
                View.MusicSlider.SliderValue = View.MusicSlider.SliderValue;
            else
                ChangeAudioMixerVolume(AudioMixerGroupEnum.Music);
        }

        private void ChangeAudioMixerVolume(AudioMixerGroupEnum id)
        {
            var setup = _staticDataService.GetAudioMixerGroupSetup(id);
            _audioMixer.SetFloat(setup.AudioMixerGroup.name, -80);
        }
    }
}