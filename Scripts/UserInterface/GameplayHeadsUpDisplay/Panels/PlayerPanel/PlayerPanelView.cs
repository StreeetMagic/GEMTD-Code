using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.GameplayHeadsUpDisplay.PlayerPanel
{
    public class PlayerPanelView : View
    {
        [Required]
        [field: SerializeField]
        public Slider HealthBar { get; private set; }

        [Required]
        [field: SerializeField]
        public TextMeshProUGUI HealthText { get; private set; }
    }
}