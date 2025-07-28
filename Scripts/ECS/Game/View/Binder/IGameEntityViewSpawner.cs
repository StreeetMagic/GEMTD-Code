using UnityEngine;

namespace Game.Binder
{
    public interface IGameEntityViewSpawner
    {
        void Spawn(GameEntity entity, Vector3 at);
    }
}