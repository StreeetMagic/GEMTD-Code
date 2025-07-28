using System.Collections.Generic;
using System.Linq;
using Services.PersistentProgresses;
using UnityEngine;

namespace Services.SaveLoadServices
{
    public class PlayerPrefsSaveLoad : ISaveLoadService
    {
        private readonly PersistentProgressService _progressService;

        public PlayerPrefsSaveLoad(PersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public List<IProgressReader> ProgressReaders { get; } = new();
        public bool HasSavedProgress => PlayerPrefs.HasKey(ProgressKey());

        public void SaveProgress()
        {
            UpdateProgressWriters();
            WritePlayerPrefs();
        }

        public void LoadProgress()
        {
            ReadPlayerPrefs();
            UpdateProgressReaders();
        }

        public void DeleteSaves()
        {
            PlayerPrefs.DeleteKey(ProgressKey());
        }

        private string ProgressKey() =>
            "_progress";

        private void UpdateProgressReaders()
        {
            foreach (IProgressReader progressReader in ProgressReaders)
                progressReader.ReadProgress(_progressService.ProjectProgress);
        }

        private void UpdateProgressWriters()
        {
            foreach (IProgressWriter progressWriter in ProgressReaders
                         .OfType<IProgressWriter>()
                         .ToList())

                progressWriter.WriteProgress(_progressService.ProjectProgress);
        }

        private void WritePlayerPrefs() =>
            PlayerPrefs.SetString(ProgressKey(), JsonUtility.ToJson(_progressService.ProjectProgress));

        private void ReadPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(ProgressKey()))
                _progressService.LoadProgress(PlayerPrefs.GetString(ProgressKey()));
            else
                _progressService.SetDefault();
        }
    }
}