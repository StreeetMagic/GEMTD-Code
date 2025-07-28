using Game.Registrars;
using UnityEngine;

namespace Game.Towers
{
    public class ShootingPointWorldPositionRegistrar : EntityComponentRegistrar
    {
        public Transform Transform;

        public override void RegisterComponents()
        {
            Entity.AddShootingPointWorldPosition(Transform);
        }

        public override void UnregisterComponents()
        {
            Entity.RemoveShootingPointWorldPosition();
        }
    }
}