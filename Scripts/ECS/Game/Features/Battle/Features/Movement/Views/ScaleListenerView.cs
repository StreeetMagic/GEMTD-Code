using UnityEngine;

namespace Game.Battle
{
    public class ScaleListenerView : EntityDependant, IScaleListener
    {
        private void Start()
        {
            if (Entity == null)
                return;

            Entity.AddScaleListener(this);
            OnScale(Entity, Entity.Scale);
        }

        public void OnScale(GameEntity entity, Vector3 value)
        {
            transform.localScale = value; 
        }
    }
}