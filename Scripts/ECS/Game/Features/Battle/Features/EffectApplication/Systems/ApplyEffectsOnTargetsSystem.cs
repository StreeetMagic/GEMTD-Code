using System.Collections.Generic;
using Entitas;

namespace Game.Battle.EffectApplication
{
    public class ApplyEffectsOnTargetsSystem : IExecuteSystem
    {
        private readonly IEffectFactory _effectFactory;
        private readonly IGroup<GameEntity> _armaments;
        private readonly List<GameEntity> _buffer = new List<GameEntity>(16);

        public ApplyEffectsOnTargetsSystem(GameContext game, IEffectFactory effectFactory)
        {
            _effectFactory = effectFactory;

            _armaments = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.TargetBuffer,
                    GameMatcher.EffectSetups,
                    GameMatcher.ReadyToApplyEffect
                )
                .NoneOf(
                    GameMatcher.Processed
                )
            );
        }

        public void Execute()
        {
            foreach (GameEntity armament in _armaments.GetEntities(_buffer))
            foreach (EffectSetup setup in armament.EffectSetups)
            foreach (int targetId in armament.TargetBuffer)
            {
                _effectFactory.CreateEffect(setup, ProducerId(armament), targetId);
                armament.isReadyToApplyEffect = false;
                armament.isProcessed = true;
            }
        }

        private static int ProducerId(GameEntity entity)
        {
            return entity.hasProducerId
                ? entity.ProducerId
                : entity.Id;
        }
    }
}