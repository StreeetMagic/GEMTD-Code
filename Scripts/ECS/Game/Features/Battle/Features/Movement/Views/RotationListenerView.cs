using UnityEngine;

namespace Game.Battle
{
    public class RotationListenerView : EntityDependant, IRotationListener
    {
        public Transform Transform;
        
        private void Start()
        {
            if (!Transform)
                Transform = transform;
            
            Entity.AddRotationListener(this);
            OnRotation(Entity, Entity.Rotation);
        }

        public void OnRotation(GameEntity entity, Quaternion value)
        {
            Transform.rotation = value;
        }
    }
}