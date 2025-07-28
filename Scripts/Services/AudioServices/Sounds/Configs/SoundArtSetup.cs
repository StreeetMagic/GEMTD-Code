using System;
using System.Collections.Generic;
using Infrastructure;
using Services.AudioServices.AudioMixers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Services.AudioServices.Sounds.Configs
{
  [Serializable]
  public class SoundArtSetup : ArtSetup<SoundEnum>
  {
    [FormerlySerializedAs("AudioMixerGroupId")]
    public AudioMixerGroupEnum AudioMixerGroupEnum;

    [Range(0f, 1f)] public float Volume;

    public List<AudioClipWithChance> AudioClipsWithChance;
  }
}