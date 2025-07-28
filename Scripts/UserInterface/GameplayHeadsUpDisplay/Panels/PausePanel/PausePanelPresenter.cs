using Services.AudioServices;
using Services.StaticData;
using Services.Times;
using UnityEngine;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay
{
    public class PausePanelPresenter :
        Presenter<PausePanelView>,
        IInitializable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ITimeService _timeService;
        private readonly AudioService _audioService;

        public PausePanelPresenter(
            PausePanelView view,
            IStaticDataService staticDataService,
            ITimeService timeService,
            AudioService audioService) : base(view)
        {
            _staticDataService = staticDataService;
            _timeService = timeService;
            _audioService = audioService;
        }

        public void Initialize()
        {
            View.PauseButton.Text.text = "II";

            View.PauseButton.Button.onClick.AddListener(() =>
            {
                DisableOutlines();
                _timeService.Pause();
                _audioService.StopAll();
                View.PauseButton.OutlineImage.enabled = true;
            });

            for (var i = 0; i < _staticDataService.ProjectConfig.TimeMultipliers.Length; i++)
            {
                var multiplier = _staticDataService.ProjectConfig.TimeMultipliers[i];
                View.Buttons[i].Text.text = $"{multiplier}x";

                View.Buttons[i].Button.onClick.AddListener(() => { SetTimeMultiplier(multiplier); });
            }
        }

        public void Enable()
        {
            Show();

            DisableOutlines();

            if (_timeService.IsPaused)
            {
                _timeService.UnPause();
                _timeService.TimeMultiplier = _staticDataService.ProjectConfig.TimeMultipliers[0];
                View.Buttons[0].OutlineImage.enabled = true;
            }
            else
            {
                for (var i = 0; i < _staticDataService.ProjectConfig.TimeMultipliers.Length; i++)
                {
                    var multiplier = _staticDataService.ProjectConfig.TimeMultipliers[i];

                    if (Mathf.Approximately(multiplier, _timeService.TimeMultiplier))
                    {
                        View.Buttons[i].OutlineImage.enabled = true;
                    }
                }
            }
        }

        public void Disable()
        {
            Hide();
        }

        private void DisableOutlines()
        {
            View.PauseButton.OutlineImage.enabled = false;

            foreach (var button in View.Buttons)
                button.OutlineImage.enabled = false;
        }

        public void SetTimeMultiplier(float multiplier)
        {
            DisableOutlines();
            _timeService.TimeMultiplier = multiplier;
            _timeService.UnPause();

            for (var i = 0; i < _staticDataService.ProjectConfig.TimeMultipliers.Length; i++)
            {
                if (Mathf.Approximately(multiplier, _staticDataService.ProjectConfig.TimeMultipliers[i]))
                {
                    View.Buttons[i].OutlineImage.enabled = true;
                    break;
                }
            }
        }

        public void SetTimeMultiplierByInput(float multiplier)
        {
            DisableOutlines();
            _timeService.TimeMultiplier = multiplier;
            _timeService.UnPause();

            for (var i = 0; i < _staticDataService.ProjectConfig.TimeMultipliers.Length; i++)
            {
                if (Mathf.Approximately(multiplier, _staticDataService.ProjectConfig.TimeMultipliers[i]))
                {
                    View.Buttons[i].Button.onClick.Invoke();
                    break;
                }
            }
        }

        public void Pause()
        {
            DisableOutlines();
            _timeService.Pause();

            View.PauseButton.Button.onClick.Invoke();
        }
    }
}