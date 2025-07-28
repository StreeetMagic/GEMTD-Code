using UnityEngine;

namespace Infrastructure
{
  public abstract class ArtConfig<T> : ScriptableObject where T : class
  {
    protected abstract void Validate();

    private void OnValidate()
    {
      Validate();
    }

    public T[] Setups;
  }
}