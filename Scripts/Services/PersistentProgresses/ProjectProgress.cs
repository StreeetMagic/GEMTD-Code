using System;
using UnityEngine;
using UserInterface.SettingsMenu;

namespace Services.PersistentProgresses
{
    [Serializable]
    public class ProjectProgress
    {
        public SettingsData SettingsData;

        public ProjectProgress
        (
            SettingsData settings
        )
        {
            SettingsData = settings;
        }
    }
}