using Sirenix.OdinInspector;
using UserInterface.GameplayHeadsUpDisplay.Buttons;

namespace UserInterface.GameplayHeadsUpDisplay
{
    public class PausePanelView : View
    {
        [Required]
        public OutlineButtonView PauseButton;
        [Required]
        public OutlineButtonView[] Buttons;
    }
}