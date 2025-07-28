namespace Game.PlayerAbility.PlayerAbilityFactory
{
    public interface IPlayerAbilityFactory
    {
        void CreateSwapRequest(int playerId);
        void CreateDeactivateSwapRequest(int playerId);
        void CreatePlayerSwapAbility(int realPlayerId);
        void CreateHealThroneAbility(int playerId);
        void CreateHealThroneRequest(int playerId);
        void CreateTimeLapseRequest(int playerId);
        void CreateTimeLapseAbility(int playerId);
    }
}