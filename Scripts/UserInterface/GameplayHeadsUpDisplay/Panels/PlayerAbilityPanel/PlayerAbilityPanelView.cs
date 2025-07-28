using Sirenix.OdinInspector;
using UserInterface.GameplayHeadsUpDisplay.Buttons;

namespace UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel
{
    public class PlayerAbilityPanelView : View
    {
        [Required]
        public AbilityButtonView[] ButtonsView;
    }
}