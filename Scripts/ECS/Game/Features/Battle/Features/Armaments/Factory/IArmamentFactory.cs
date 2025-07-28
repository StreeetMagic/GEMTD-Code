using System.Collections.Generic;
using Game.Battle.Configs;
using UnityEngine;

namespace Game.Battle
{
    public interface IArmamentFactory
    {
        // GameEntity CreateVegetableBolt(int level, Vector3 at);
        // GameEntity CreateMushroom(int level, Vector3 at, float phase);
        // GameEntity CreateEffectAura(AbilityEnum parentAbilityEnum, int producerId, int level);
        // GameEntity CreateExplosion(int producerId, Vector3 at);
        public GameEntity CreateTowerBasicAttackProjectile(Vector3 at, AbilitySetup abilitySetup, List<AbilitySetup> abilitySetups, Quaternion rotation,
            int towerId);

        public GameEntity CreateSplitshot(
            Vector3 at,
            AbilitySetup abilitySetup,
            Quaternion rotation,
            int towerId
        );

        GameEntity CreateCleave(
            Vector3 at, 
            AbilitySetup abilitySetup
        );

        GameEntity CreateCleaveRequest(GameEntity armament);
    }
}