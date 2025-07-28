using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemies
{
    [CreateAssetMenu(fileName = nameof(EnemyConfig), menuName = "Configs/" + nameof(EnemyConfig))]
    public class EnemyConfig : ScriptableObject
    {
        public string Name;
        public string Description;
        public int Round;

        [SerializeField]
        public GameEntityView Prefab;

        [SerializeField]
        public float MoveSpeed;

        [SerializeField]
        public float RotationSpeed;

        [SerializeField]
        public bool IsFlyable;

        [SerializeField]
        public float Damage;

        [FormerlySerializedAs("MaxHP")]
        [SerializeField]
        public float MaxHealthPoints;

        [SerializeField]
        public float Armor;

        [SerializeField]
        public int GoldReward;
    }
}