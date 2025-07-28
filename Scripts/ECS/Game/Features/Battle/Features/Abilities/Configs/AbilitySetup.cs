using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battle.Configs
{
    [Serializable]
    public class AbilitySetup
    {
        public AbilityEnum AbilityEnum = AbilityEnum.BasicAttack;
        
        public GameLoopStateEnum[] CanUseIn;
        public int Cost;
        public Sprite Icon;
        public string Description = "ABOBA";
        public int Level;
        public string Name;

        public float Cooldown = 1;

        public GameEntityView ViewPrefab;
        public GameEntityView ExplosionPrefab;
        public GameEntityView MuzzleFlashPrefab;

        public List<EffectSetup> EffectSetups;
        public List<StatusSetup> StatusSetups;
    
        public ProjectileSetup ProjectileSetup;
        public float AttackSpeed;
        public AuraSetup AuraSetup;
    }
}