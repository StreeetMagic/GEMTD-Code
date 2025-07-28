using System;
using Game.Entity;
using Game.Extensions;
using Services.Identifiers;

namespace Game.Battle
{
    public class EffectFactory : IEffectFactory
    {
        private readonly IIdentifierService _identifiers;

        public EffectFactory(IIdentifierService identifiers)
        {
            _identifiers = identifiers;
        }

        public GameEntity CreateEffect(EffectSetup setup, int producerId, int targetId)
        {
            switch (setup.EffectEnum)
            {
                case EffectEnum.Unknown:
                    break;

                case EffectEnum.Damage:
                    return CreateDamage(producerId, targetId, setup.Value);

                case EffectEnum.Heal:
                    return CreateHeal(producerId, targetId, setup.Value);
            }

            throw new Exception($"Effect with type id {setup.EffectEnum} does not exist");
        }

        public GameEntity CreateDamage(int producerId, int targetId, float value)
        {
            return CreateGameEntity.Empty()
                .AddId(_identifiers.Next())
                .With(x => x.isEffect = true)
                .With(x => x.isDamageEffect = true)
                .AddEffectValue(value)
                .AddProducerId(producerId)
                .AddTargetId(targetId);
        }

        public GameEntity CreateHeal(int producerId, int targetId, float value)
        {
            return CreateGameEntity.Empty()
                .AddId(_identifiers.Next())
                .With(x => x.isEffect = true)
                .With(x => x.isHealEffect = true)
                .AddEffectValue(value)
                .AddProducerId(producerId)
                .AddTargetId(targetId);
        }
    }
}