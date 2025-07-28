using Game.Battle;
using TMPro;
using UnityEngine.UI;

namespace UserInterface.GameplayHeadsUpDisplay.Buttons
{
    public class AbilityButtonView : View
    {
        public Button Button;
        public Image Icon;
       // public TMP_Text Level;
        public TMP_Text Cost;
        public TMP_Text Cooldown;
        public Image OutlineActiveAbility;
        public Image CooldownSlider;
        public TMP_Text NameText;
        
        public AbilityEnum PlayerAbilityEnum { get; set; }
    }
}