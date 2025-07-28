using Game.GameMainFeature;
using UnityEngine;

namespace Game.Enemies
{
    [CreateAssetMenu(fileName = nameof(EnemyCoefficientConfig), menuName = "Configs/" + nameof(EnemyCoefficientConfig))]
    public class EnemyCoefficientConfig : ScriptableObject
    {
        public GameModeEnum GameModeEnum;

        public EnemyCoefficient EnemyCoefficient;
    }
}