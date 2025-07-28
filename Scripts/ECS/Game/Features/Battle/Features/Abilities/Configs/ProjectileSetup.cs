using System;

namespace Game.Battle.Configs
{
  [Serializable]
  public class ProjectileSetup
  {
    public int ProjectileCount = 1;
    
    public float MoveSpeed;
    public int Pierce = 1;
    public float ContactRadius;
    public float Lifetime;
    
    public float OrbitRadius;
  }
}