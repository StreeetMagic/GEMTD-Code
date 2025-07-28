using System.Collections.Generic;
using Entitas;
using Services.AudioServices;
using Services.AudioServices.Sounds;

namespace Game.Towers
{
    public class SpiritPlacedReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly AudioService _audioService;
        
        public SpiritPlacedReactiveSystem(Contexts contexts, AudioService audioService) : base(contexts.game)
        {
            _audioService = audioService;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.TowerSpirit);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isTowerSpirit;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var dummy in entities)
            {
                _audioService.Play(SoundEnum.TowerPlaced);
            }
        }
    }
}