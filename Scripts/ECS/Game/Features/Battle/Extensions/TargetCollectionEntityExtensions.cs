namespace Game.Battle
{
    public static class TargetCollectionEntityExtensions
    {
        public static GameEntity RemoveTargetCollectionComponents(this GameEntity entity)
        {
            if (entity.hasTargetBuffer)
                entity.RemoveTargetBuffer();

            if (entity.hasCollectTargetsInterval)
                entity.RemoveCollectTargetsInterval();

            if (entity.hasCollectTargetsTimer)
                entity.RemoveCollectTargetsTimer();

            entity.isReadyToCollectTargets = false;

            return entity;
        }
    }
}