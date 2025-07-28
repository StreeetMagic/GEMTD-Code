using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Battle
{
    public enum StatEnum
    {
        None = 0,

        MaxHeathPoints = 3,
        AttackCooldown = 5,
        AttackRange = 6,
        AttackTimer = 7,
        MoveSpeed = 8,
        RotationSpeed = 10,
        Armor = 11,
        BasicAttackAdditionalProjectiles = 12,
        AttackSpeed = 13,
    }

    public static class InitStats
    {
        public static Dictionary<StatEnum, float> EmptyStatDictionary()
        {
            return Enum.GetValues(typeof(StatEnum))
                .Cast<StatEnum>()
                .Except(new[] { StatEnum.None })
                .ToDictionary(x => x, _ => 0f);
        }
    }
}