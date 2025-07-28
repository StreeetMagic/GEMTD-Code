using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace UserInterface.GameplayHeadsUpDisplay.TimerPanel
{
    public class TimerPanelView : View
    {
        [Required]
        [field: SerializeField]
        public TextMeshProUGUI CurrentRound { get; private set; }

        [Required]
        [field: SerializeField]
        public TextMeshProUGUI CurrentRoundTimerText { get; private set; }

        [Required]
        [field: SerializeField]
        public TextMeshProUGUI TotalGameTimeText { get; private set; }

        [Required]
        [field: SerializeField]
        public TextMeshProUGUI AliveEnemies { get; private set; }
    }
}