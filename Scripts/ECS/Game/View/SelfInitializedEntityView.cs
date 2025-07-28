using Game.Entity;
using Services.Identifiers;
using UnityEngine;
using Zenject;

namespace Game
{
  public class SelfInitializedEntityView : MonoBehaviour
  {
    public GameEntityView GameEntityView;
    private IIdentifierService _identifiers;

    [Inject]
    private void Construct(IIdentifierService identifiers)
    {
      _identifiers = identifiers;
    }

    private void Awake()
    {
      GameEntityView = GetComponent<GameEntityView>();

      GameEntity entity =
        CreateGameEntity
          .Empty()
          .AddId(_identifiers.Next());

      GameEntityView.SetEntity(entity);
    }
  }
}