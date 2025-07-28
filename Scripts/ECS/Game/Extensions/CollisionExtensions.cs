using UnityEngine;

namespace Game.Extensions
{
  public enum CollisionLayerEnum
  {
    Hero = 6,
    Enemy = 7,
    Collectable = 9,
    Clicker = 10,
  }
  
  public static class CollisionExtensions
  {
    public static bool Matches(this Collider2D collider, LayerMask layerMask) =>
      ((1 << collider.gameObject.layer) & layerMask) != 0;

    public static int AsMask(this CollisionLayerEnum layerEnum) =>
      1 << (int)layerEnum;
  }
}