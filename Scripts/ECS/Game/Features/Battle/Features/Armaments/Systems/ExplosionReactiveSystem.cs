using System.Collections.Generic;
using Entitas;
using Services.AudioServices;
using Services.AudioServices.Sounds;

namespace Game.Battle
{
    public class ExplosionReactiveSystem : ReactiveSystem<GameEntity>
    {
        private AudioService _audioService;
        public ExplosionReactiveSystem(Contexts contexts, AudioService audioService) : base(contexts.game)
        {
            _audioService = audioService;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Explosion);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isExplosion;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                _audioService.Play(SoundEnum.Explosion);
            }
        }
    }
}