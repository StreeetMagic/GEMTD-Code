using System.Collections.Generic;
using UnityEngine.Audio;
using Zenject;

namespace Services.AudioServices
{
  public class AudioMixerGroupWrapper : ITickable
  {
    private readonly List<AudioSourceWrapper> _audioSourceWrappers;
    private readonly float _cooldown;

    private float _timeLeft;

    public AudioMixerGroupWrapper(AudioMixerGroup audioMixerGroup, float cooldown)
    {
      AudioMixerGroup = audioMixerGroup;
      _cooldown = cooldown;
      _audioSourceWrappers = new List<AudioSourceWrapper>();
    }

    public AudioMixerGroup AudioMixerGroup { get; }

    public AudioSourceWrapper GetOrNull()
    {
      if (_timeLeft > 0)
        return null;

      AudioSourceWrapper audioSourceWrapper = null;

      _timeLeft = _cooldown;

      foreach (AudioSourceWrapper wrapper in _audioSourceWrappers)
      {
        if (wrapper.IsActive)
          continue;

        audioSourceWrapper = wrapper;
        break;
      }

      return audioSourceWrapper;
    }

    public void Add(AudioSourceWrapper audioSourceWrapper)
    {
      _audioSourceWrappers.Add(audioSourceWrapper);
    }

    public void Tick()
    {
      _timeLeft -= UnityEngine.Time.deltaTime;

      foreach (AudioSourceWrapper audioSourceWrapper in _audioSourceWrappers)
      {
        if (audioSourceWrapper.IsActive)
          audioSourceWrapper.Tick();
      }
    }

    public void StopAll()
    {
      foreach (AudioSourceWrapper audioSourceWrapper in _audioSourceWrappers)
      {
        audioSourceWrapper.IsActive = false;
        audioSourceWrapper.AudioSource.Stop();
        audioSourceWrapper.AudioSource.clip = null;
        audioSourceWrapper.AudioSource.loop = false;
      }
    }

    public void Mute()
    {
      foreach (AudioSourceWrapper audioSourceWrapper in _audioSourceWrappers)
        audioSourceWrapper.AudioSource.mute = true;
    }

    public void UnMute()
    {
      foreach (AudioSourceWrapper audioSourceWrapper in _audioSourceWrappers)
        audioSourceWrapper.AudioSource.mute = false;
    }
  }
}