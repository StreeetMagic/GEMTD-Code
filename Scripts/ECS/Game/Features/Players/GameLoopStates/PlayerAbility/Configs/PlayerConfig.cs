using System.Collections.Generic;
using Game.Battle.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.PlayerAbility.Configs
{
    [CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "Configs/" + nameof(PlayerConfig))]
    public class PlayerConfig : SerializedScriptableObject
    {
        public List<AbilitySetup> PlayerAbilitySetup;
    }
}