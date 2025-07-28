using Entitas;
using UnityEngine;

namespace Game.Battle
{
    public class ProcessDamageEffectSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _effects;

        public ProcessDamageEffectSystem(GameContext game)
        {
            _effects = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.DamageEffect,
                    GameMatcher.EffectValue,
                    GameMatcher.TargetId));
        }

        public void Execute()
        {
            foreach (GameEntity effect in _effects)
            {
                GameEntity target = effect.Target();

                effect.isProcessed = true;

                if (target.isDead)
                    continue;

                float damage = effect.EffectValue;

                if (target.hasArmor)
                {
                    float armor = target.Armor;
                    float multiplier = 1f - (0.06f * armor) / (1f + 0.06f * Mathf.Abs(armor));
                    damage *= multiplier;
                }

                target.ReplaceCurrentHealthPoints(target.CurrentHealthPoints - damage);

                // if(target.hasDamageTakenAnimator)
                //   target.DamageTakenAnimator.PlayDamageTaken();
            }
        }
    }
}