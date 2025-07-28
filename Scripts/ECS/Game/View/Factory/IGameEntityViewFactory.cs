using UnityEngine;

namespace Game.Factory
{
    public interface IGameEntityViewFactory
    {
        void Create(GameEntity entity, Vector3 at);
    }
}