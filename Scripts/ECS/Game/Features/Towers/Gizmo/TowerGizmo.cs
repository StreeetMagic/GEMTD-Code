using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Towers
{
    public class TowerGizmo : MonoBehaviour
    {
        [FormerlySerializedAs("entityView")]
        public GameEntityView EntityView;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            
            float radius = EntityView.Entity.AttackRange;
        
            float height = .1f;
        
            Gizmos.color = Color.red;
        
            int segments = 32;
            float halfHeight = height * 0.5f;
        
            Vector3 up = transform.up;
            Vector3 center = transform.position;
        
            Vector3 topCenter = center + up * halfHeight;
            Vector3 bottomCenter = center - up * halfHeight;
        
            float angleStep = (Mathf.PI * 2f) / segments;
        
            for (int i = 0; i < segments; i++)
            {
                float currentAngle = i * angleStep;
                float nextAngle = (i + 1) * angleStep;
        
                Vector3 currentPos = new Vector3(Mathf.Cos(currentAngle) * radius, 0f, Mathf.Sin(currentAngle) * radius);
                Vector3 nextPos = new Vector3(Mathf.Cos(nextAngle) * radius, 0f, Mathf.Sin(nextAngle) * radius);
        
                Vector3 topPoint = topCenter + transform.rotation * currentPos;
                Vector3 topPointNext = topCenter + transform.rotation * nextPos;
        
                Vector3 bottomPoint = bottomCenter + transform.rotation * currentPos;
                Vector3 bottomPointNext = bottomCenter + transform.rotation * nextPos;
        
                Gizmos.DrawLine(topPoint, topPointNext);
                Gizmos.DrawLine(bottomPoint, bottomPointNext);
                Gizmos.DrawLine(topPoint, bottomPoint);
            }
        }
    }
}