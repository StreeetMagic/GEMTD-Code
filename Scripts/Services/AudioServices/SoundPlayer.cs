using UnityEngine;

namespace Services.AudioServices
{
  public class SoundPlayer
  {
    public void Play(AudioClip audioClip, AudioSource source, float volume, Vector3 at, bool loop)
    {
      if (loop)
      {
        source.loop = true;
        source.clip = audioClip;
        source.volume = volume;
        source.Play();
      }
      else
      {
        source.transform.position = at;
        source.PlayOneShot(audioClip, volume);
      }
    }
  }
}