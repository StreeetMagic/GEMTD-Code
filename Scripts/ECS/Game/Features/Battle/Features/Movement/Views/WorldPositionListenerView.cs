using UnityEngine;

namespace Game.Battle
{
    public class WorldPositionListenerView : EntityDependant, IWorldPositionListener
    {
        private void Start()
        {
            if (Entity == null) 
                return;
            
            Entity.AddWorldPositionListener(this);
            OnWorldPosition(Entity, Entity.WorldPosition);
        }

        public void OnWorldPosition(GameEntity entity, Vector3 value)
        {
            transform.position = value;
        }
    }
}