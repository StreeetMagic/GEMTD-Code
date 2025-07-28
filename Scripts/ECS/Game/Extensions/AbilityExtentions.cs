using System;
using Game.Battle;

namespace Game.Extensions
{
    public static class AbilityExtentions
    {
        public static GameEntity AddAbilityComponent(this GameEntity entity, AbilityEnum abilityEnum)
        {
            switch (abilityEnum)
            {
                case AbilityEnum.Unknown:
                    break;
                
                case AbilityEnum.BasicAttack:
                    entity.isBasicAttackAbility = true;
                    break;
                
                case AbilityEnum.Split:
                    entity.isSplitShotAbility = true;
                    break;
                
                case AbilityEnum.Cleave:
                    entity.isCleaveAbility = true;
                    break;
                
                case AbilityEnum.Pierce:
                    entity.isPierceAbility = true;
                    break;
                
                case AbilityEnum.Poison:
                    entity.isPoisonAbility = true;
                    break;
                
                case AbilityEnum.Aura:
                    entity.isAuraAbility = true;
                    break;
                
                case AbilityEnum.Slow:
                    entity.isSlowAbility = true;
                    break;
                
                case AbilityEnum.Speed:
                    entity.isSpeedAbility = true;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityEnum), abilityEnum, null);
            }

            return entity;
        }
    }
}