using UnityEngine;

namespace Game.Battle
{
    public class AuraSizeListener : EntityDependant
    {
        public Transform Container;
        private float _radiusPrev;

        private void Update()
        {
            if (Mathf.Abs(Entity.Radius - _radiusPrev) < Mathf.Epsilon)
                return;

            SetAuraScale();
        }

        private void SetAuraScale()
        {
            float scale = Entity.Radius * 2;
            Container.localScale = new Vector3(scale, scale, scale);

            _radiusPrev = Entity.Radius;
        }
    }
}