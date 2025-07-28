using UnityEngine;
using Zenject;

namespace Services.AudioServices
{
  public class AudioSourceWrapper : ITickable
  {
    public AudioSourceWrapper(AudioSource audioSource)
    {
      AudioSource = audioSource;
    }

    public AudioSource AudioSource { get; }
    public bool IsActive { get; set; }

    public void Tick()
    {
      if (AudioSource.isPlaying == false)
        IsActive = false;
    }
  }
}