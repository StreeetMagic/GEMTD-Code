using Entitas;

namespace Game.Battle
{
    public class FinalizeProcessedArmamentsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _armaments;
        private readonly VisualEffectFactory _factories;

        public FinalizeProcessedArmamentsSystem(GameContext game, VisualEffectFactory factories)
        {
            _factories = factories;
            _armaments = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Armament,
                    GameMatcher.Processed));
        }

        public void Execute()
        {
            foreach (GameEntity armament in _armaments)
            {
                armament.RemoveTargetCollectionComponents();
                armament.isDestructed = true;

                if (armament.hasProducerId && armament.hasWorldPosition) //TODO надо рефакторить
                    _factories.CreateExplosionVisualEffect(armament.WorldPosition, armament.ProducerId);
            }
        }
    }
}