using System;

namespace Infrastructure
{
  [Serializable]
  public class ArtSetup<T> where T : Enum
  {
    public T Id;
  }
}