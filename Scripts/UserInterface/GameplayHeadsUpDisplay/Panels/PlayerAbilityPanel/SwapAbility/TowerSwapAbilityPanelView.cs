using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel.SwapAbility
{
    public class TowerSwapAbilityPanelView : View
    {
        [Required]
        public Button StopUseAbilityButton;

        [Required]
        public Button ButtonAprrove;

        [Required]
        public Image FirstElementImage;

        [Required]
        public Image SecondElementImage;
        
        [Required]
        public TextMeshProUGUI FirstElementTitleName;

        [Required]
        public TextMeshProUGUI FirstElementDescription;

        [Required]
        public TextMeshProUGUI SecondElementTitleName;

        [Required]
        public TextMeshProUGUI SecondElementDescription;
    }
}