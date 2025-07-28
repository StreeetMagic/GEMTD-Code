using Entitas.Unity;
using Game.Registrars;
using Services.Collisions;
using UnityEngine;
using Zenject;

namespace Game
{
    [SelectionBase]
    public class GameEntityView : MonoBehaviour, IGameEntityView
    {
        private ICollisionRegistry _collisionRegistry;

        public GameEntity Entity { get; private set; }

        [Inject]
        private void Construct(ICollisionRegistry collisionRegistry)
        {
            _collisionRegistry = collisionRegistry;
        }

        public void SetEntity(GameEntity entity)
        {
            Entity = entity;
            Entity.AddView(this);

            gameObject.Link(entity);

            foreach (IEntityComponentRegistrar registrar in GetComponentsInChildren<IEntityComponentRegistrar>())
                registrar.RegisterComponents();

            foreach (Collider collider2d in GetComponentsInChildren<Collider>(includeInactive: true))
                _collisionRegistry.Register(collider2d.GetInstanceID(), Entity);
        }

        public void ReleaseEntity()
        {
            foreach (IEntityComponentRegistrar registrar in GetComponentsInChildren<IEntityComponentRegistrar>())
                registrar.UnregisterComponents();

            foreach (Collider coll in GetComponentsInChildren<Collider>(includeInactive: true))
                _collisionRegistry.Unregister(coll.GetInstanceID());

            gameObject.Unlink();
            Entity = null;
        }
    }
}