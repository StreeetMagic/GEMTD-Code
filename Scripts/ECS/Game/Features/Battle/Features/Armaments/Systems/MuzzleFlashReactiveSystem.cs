using System.Collections.Generic;
using Entitas;
using Services.AudioServices;

namespace Game.Battle
{
    public class MuzzleFlashReactiveSystem : ReactiveSystem<GameEntity>
    {
        private AudioService _audioService;
        private GameContext _gameContext;
        
        public MuzzleFlashReactiveSystem(Contexts contexts, AudioService audioService) : base(contexts.game)
        {
            _audioService = audioService;
            _gameContext = contexts.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.MuzzleFlash);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isMuzzleFlash;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                var tower = _gameContext.GetEntityWithId(entity.ProducerId);
                _audioService.PlayByTowerEnum(tower.TowerEnum);
            }
        }
    }
}