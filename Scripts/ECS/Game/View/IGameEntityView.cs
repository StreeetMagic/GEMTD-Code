using UnityEngine;

namespace Game
{
  public interface IGameEntityView
  {
     void ReleaseEntity();
    
    GameObject gameObject { get; }
  }
}