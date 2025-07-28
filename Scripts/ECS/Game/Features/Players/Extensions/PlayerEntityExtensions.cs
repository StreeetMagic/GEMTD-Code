namespace Game.Extensions
{
    public static class PlayerEntityExtensions
    {
        public static GameEntity GainGold(this GameEntity player, int gold)
        {
            if (player.hasGold == false) 
                throw new System.Exception("Player has no gold component");
            
            return player.ReplaceGold(player.Gold + gold);
        }
        
        public static GameEntity LoseGold(this GameEntity player, int gold)
        {
            if (player.hasGold == false) 
                throw new System.Exception("Player has no gold component");
            
            return player.ReplaceGold(player.Gold - gold);
        }
    }
}