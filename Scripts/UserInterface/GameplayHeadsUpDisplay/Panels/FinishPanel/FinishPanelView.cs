using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace UserInterface.GameplayHeadsUpDisplay.FinishPanel
{
    public class FinishPanelView : View
    {
        [Required]
        public TextMeshProUGUI TopText;
        [Required]
        public TextMeshProUGUI LevelText;
        [Required]
        public TextMeshProUGUI KillText;
        [Required]
        public TextMeshProUGUI DurationText;
        [Required]
        public TextMeshProUGUI CheatsText;

        [Required]
        public Button RestartButton;
    }
}