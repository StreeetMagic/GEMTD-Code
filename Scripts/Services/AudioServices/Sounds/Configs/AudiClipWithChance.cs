using System;
using UnityEngine;

namespace Services.AudioServices.Sounds.Configs
{
    [Serializable]
    public struct AudioClipWithChance
    {
        [SerializeField] 
        public AudioClip AudioClip;
        
        [SerializeField]
        [Range(0, 50)] 
        public int Chance;
        
        public AudioClipWithChance(AudioClip clip, int chance = 5)
        {
            AudioClip = clip;
            Chance = Mathf.Clamp(chance, 0, 50);
        }
    }
}