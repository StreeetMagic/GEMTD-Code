using Entitas;
using Game.Entity;
using Services.Identifiers;
using Zenject;

namespace Game.Inputs
{
  public class InitializeInputSystem : IInitializeSystem
  {
    private IIdentifierService _identifiers;

    [Inject]
    private void Construct(IIdentifierService identifiers)
    {
      _identifiers = identifiers;
    }
  
    public void Initialize()
    {
      CreateGameEntity.Empty()
        .AddId(_identifiers.Next())
        .isUserInput = true;
    }
  }
}