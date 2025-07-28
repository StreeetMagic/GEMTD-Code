using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.GameplayHeadsUpDisplay.CheatsPanel
{
    public class CheatsPanelView : View
    {
        [field: SerializeField]
        [Required]
        public Button RestartButton { get; private set; }

        [field: SerializeField]
        [Required]
        public Button HealButton { get; private set; }

        [field: SerializeField]
        [Required]
        public Button MoneyButton { get; private set; }

        [field: SerializeField]
        [Required]
        public Button KillButton { get; private set; }
    }
}