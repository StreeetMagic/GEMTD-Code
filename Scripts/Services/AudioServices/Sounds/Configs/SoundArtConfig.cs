using Infrastructure;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Services.AudioServices.Sounds.Configs
{
    [CreateAssetMenu(fileName = "Sound", menuName = "ArtConfigs/Sound")]
    public class SoundArtConfig : ArtConfig<SoundArtSetup>
    {
        protected override void Validate()
        {
        }
    }
}