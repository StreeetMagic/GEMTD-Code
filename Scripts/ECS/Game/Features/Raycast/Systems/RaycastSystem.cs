using System.Collections.Generic;
using Entitas;
using Services.Physics;
using UnityEngine.EventSystems;

namespace Game.Raycast
{
    public class RaycastSystem : IExecuteSystem
    {
        private readonly IPhysicsService _physicsService;
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _raycasted;
        private readonly List<GameEntity> _buffer = new(1);

        public RaycastSystem(
            IPhysicsService physicsService,
            GameContext game
        )
        {
            _physicsService = physicsService;
            _game = game;

            _raycasted = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CanRaycast,
                GameMatcher.Raycasting
            ));
        }

        public void Execute()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            foreach (GameEntity raycasted in _raycasted.GetEntities(_buffer))
                raycasted.isRaycasting = false;

            var camera = _game.cameraEntity;
            
            if (camera == null)
                return;

            GameEntity raycastEntity = _physicsService.Raycast(camera.Ray.origin, camera.Ray.direction);

            if (raycastEntity == null)
                return;

            if (raycastEntity.isCanRaycast)
            {
                raycastEntity.isRaycasting = true;
            }
        }
    }
}