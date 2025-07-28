namespace Game.Registrars
{
  public interface IEntityComponentRegistrar
  {
    void RegisterComponents();
    void UnregisterComponents();
  }
}