namespace Game.Entity
{
    public static class CreateGameEntity
    {
        public static GameEntity Empty()
        {
            return Contexts.sharedInstance.game.CreateEntity();
        }
    }
}