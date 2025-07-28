using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public abstract class EntityDependant : MonoBehaviour
    {
        public GameEntityView GameEntityView;

        protected GameEntity Entity => GameEntityView != null
            ? GameEntityView.Entity
            : null;

        private void Awake()
        {
            if (!GameEntityView)
                GameEntityView = GetComponent<GameEntityView>();
        }
    }
}