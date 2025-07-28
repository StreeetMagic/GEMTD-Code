using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UserInterface.GameplayHeadsUpDisplay.Buttons;

namespace UserInterface.GameplayHeadsUpDisplay
{
    public class TowerMergePanelView : View
    {
        [Required]
        public Button SelectButton;

        [Required]
        public Button DowngradeButton;

        [Required]
        public Button[] UpgradeButton;
        
        [Required]
        public Button BackButton;

        [Required]
        public TextMeshProUGUI[] Texts;

        [Required]
        public TextMeshProUGUI SelectButtonText;

        [Required]
        public TextMeshProUGUI[] Descriptions;

        [Required] 
        public Image[] Images;

        [Required] 
        public Image SelectButtonImage;
        public TextMeshProUGUI SelectButtonDescription;
        
        [Required]
        public ButtonView[] ButtonViews;
        
        [FormerlySerializedAs("CurrentButtonView")]
        public ButtonView SelectButtonView;
        public ButtonView DowngradeButtonView;
    }
}