namespace Game.Battle
{
    public struct StatusKey
    {
        public readonly int TargetId;
        public readonly StatusEnum Enum;

        public StatusKey(int targetId, StatusEnum @enum)
        {
            TargetId = targetId;
            Enum = @enum;
        }
    }
}