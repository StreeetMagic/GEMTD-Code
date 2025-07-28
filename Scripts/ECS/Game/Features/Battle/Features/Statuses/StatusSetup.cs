using System;
using UnityEngine.Serialization;

namespace Game.Battle
{
    [Serializable]
    public class StatusSetup
    {
        [FormerlySerializedAs("StatusTypeId")]
        public StatusEnum StatusEnum;

        public float Value;
        public float Duration;
        public float Period;
        public bool IsPermanent;
    }
}