using System;
using Sirenix.OdinInspector;

namespace Game.Battle.Configs
{
    [Serializable]
    public class AuraSetup
    {
        public StatEnum TargetStatType;
        
        [HideIf(nameof(TargetStatType), StatEnum.None)]
        public StatEffectSetup Setup;
    }
    
    [Serializable]
    public struct StatEffectSetup
    {
        public float Value;
        public float Radius;
        public float Interval;
        public bool IsPermanent;
    }
}