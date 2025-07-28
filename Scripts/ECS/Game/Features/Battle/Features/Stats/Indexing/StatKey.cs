namespace Game.Battle
{
    public struct StatKey
    {
        public readonly int TargetId;
        public readonly StatEnum Stat;

        public StatKey(int targetId, StatEnum stat)
        {
            TargetId = targetId;
            Stat = stat;
        }
    }
}