using UnityEngine;
using UserInterface.SettingsMenu;

namespace Services.PersistentProgresses
{
    public class PersistentProgressService
    {
        public ProjectProgress ProjectProgress { get; private set; }

        public void LoadProgress(string getString) =>
            ProjectProgress = JsonUtility.FromJson<ProjectProgress>(getString);

        public void SetDefault()
        {
            ProjectProgress = new ProjectProgress(new SettingsData());
        }
    }
}