using System.Collections.Generic;
using Entitas;
using Services.AudioServices;
using Services.AudioServices.Sounds;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemyKilledReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly AudioService _audioService;
        private readonly VisualEffectFactory _effectFactory;
        public EnemyKilledReactiveSystem(Contexts contexts, AudioService audioService, VisualEffectFactory effectFactory) : base(contexts.game)
        {
            _audioService = audioService;
            _effectFactory = effectFactory;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Dead.Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isEnemy && entity.isDead;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                _audioService.Play(SoundEnum.EnemyKilled, false);
                _effectFactory.CreateCoinDropsEffect(entity.WorldPosition, Quaternion.identity, entity.Id);
            }
        }
    }
}