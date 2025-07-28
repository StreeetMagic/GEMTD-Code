using Entitas;
using UnityEngine;

namespace Game.Battle
{
    public class ProcessHealEffectSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _effects;

        public ProcessHealEffectSystem(GameContext game)
        {
            _effects = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.HealEffect,
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

                if (target.hasCurrentHealthPoints && target.hasMaxHealthPoints)
                {
                    float newValue = Mathf.Min(target.CurrentHealthPoints + effect.EffectValue, target.MaxHealthPoints);
                    target.ReplaceCurrentHealthPoints(newValue);
                }
            }
        }
    }
}