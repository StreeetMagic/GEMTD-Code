using Game.Registrars;
using UnityEngine;

namespace Game.Enemies
{
    public class TargetPointWorldPositionRegistrar : EntityComponentRegistrar, IWorldPositionListener
    {
        public Transform Transform;

        public void Start()
        {
            Entity.AddWorldPositionListener(this);
            OnWorldPosition(Entity, Entity.WorldPosition);
        }

        public override void RegisterComponents()
        {
            Entity.AddTargetPointWorldPosition(Transform.position);
        }

        public override void UnregisterComponents()
        {
            Entity.RemoveTargetPointWorldPosition();
        }

        public void OnWorldPosition(GameEntity entity, Vector3 value)
        {
            Entity.ReplaceTargetPointWorldPosition(Transform.position);
        }
    }
}