using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace UserInterface.GameplayHeadsUpDisplay.ObjectInfoPanel
{
    public class ObjectInfoPanelView : View
    {
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Armor;
        public TextMeshProUGUI Damage;
        public TextMeshProUGUI MoveSpeed;
        public TextMeshProUGUI AttackSpeed;
        public TextMeshProUGUI Health;
        
        public Slider HealthSlider;
    }
}