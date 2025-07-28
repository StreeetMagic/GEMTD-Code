using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Towers
{
    [CreateAssetMenu(fileName = nameof(TowerConfig), menuName = "Configs/" + nameof(TowerConfig))]
    public class TowerConfig : SerializedScriptableObject
    {
        public string Name;
        public string Description;
        public TowerEnum TowerEnum;
        public TowerEnum[] Recipe;
        public TowerEnum DowngradeTo;
        public Color Color;
        public GameEntityView SpiritPlacePrefab ;

        public Sprite Sprite;

        [field: Button("Basic Attack", ButtonSizes.Gigantic)]
#if UNITY_EDITOR
        [field: GUIColor(nameof(GetBasicAttackColor))]
#endif
        [field: SerializeField]
        public AbilitySetup BasicAttackSetup { get; protected set; }

        public List<AbilitySetup> Abilities;

#if UNITY_EDITOR
        private Color GetBasicAttackColor()
        {
            return BasicAttackSetup.AbilityEnum == AbilityEnum.BasicAttack
                ? Color
                : Color.white;
        }
#endif
    }
}