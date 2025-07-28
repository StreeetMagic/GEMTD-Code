namespace Game.Battle
{
    public interface IEffectFactory
    {
        GameEntity CreateEffect(EffectSetup setup, int producerId, int targetId);
        GameEntity CreateDamage(int producerId, int targetId, float value);
        GameEntity CreateHeal(int producerId, int targetId, float value);
    }
}