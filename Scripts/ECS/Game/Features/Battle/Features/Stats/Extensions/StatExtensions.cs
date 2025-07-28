namespace Game.Battle.Extensions
{
    public static class StatExtensions
    {
        public static float Armor(this GameEntity statOwner)
        {
            var baseStat = statOwner.BaseStats[StatEnum.Armor];
            var statModifier = statOwner.StatModifiers[StatEnum.Armor];
            
            return baseStat + statModifier;
        }

        public static float MoveSpeed(this GameEntity statOwner)
        {
            var statOwnerBaseStat = statOwner.BaseStats[StatEnum.MoveSpeed];
            var statOwnerStatModifier = statOwner.StatModifiers[StatEnum.MoveSpeed];

            return (statOwnerBaseStat + statOwnerStatModifier);
        }

        public static float AttackSpeed(this GameEntity statOwner)
        {
            return (statOwner.BaseStats[StatEnum.AttackSpeed] + statOwner.StatModifiers[StatEnum.AttackSpeed]);
        }

        public static float AdditionalProjectiles(this GameEntity statOwner)
        {
            return statOwner.BaseStats[StatEnum.BasicAttackAdditionalProjectiles] +
                   statOwner.StatModifiers[StatEnum.BasicAttackAdditionalProjectiles];
        }
    }
}