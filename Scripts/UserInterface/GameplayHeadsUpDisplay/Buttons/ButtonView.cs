using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.GameplayHeadsUpDisplay.Buttons
{
    public class ButtonView : View
    {
        [Header("UI References")]
        public TextMeshProUGUI Text;

        public TextMeshProUGUI Description;
        public Image BackGroundImage;
        public Button Button;

        [Header("Color Variants")]
        public GameObject Grey;

        public GameObject Green;
        public GameObject Blue;
        public GameObject Purple;
        public GameObject Orange;
        public GameObject Red;

        private Dictionary<char, GameObject> _colorMap;

        private void Awake()
        {
            _colorMap = new Dictionary<char, GameObject>
            {
                { '1', Grey },
                { '2', Green },
                { '3', Blue },
                { '4', Purple },
                { '5', Orange },
                { '6', Red },
            };
        }

        public void SetColor(GameEntity spirit)
        {
            SetColor(spirit.TowerEnum.ToString());
        }

        public void SetColor(string variant)
        {
            foreach (var go in _colorMap.Values)
                go.SetActive(false);

            if (string.IsNullOrEmpty(variant))
            {
                Debug.LogWarning("[ButtonView] Пустая строка variant");
                return;
            }

            char key = variant[^1];

            if (_colorMap.TryGetValue(key, out var target))
                target.SetActive(true);
            else
                Debug.LogWarning($"[ButtonView] Не найден UI для варианта '{variant}' (ключ '{key}')");
        }
    }
}