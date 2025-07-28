using System.Text;
using Entitas;

namespace Game.Entity.ToStrings
{
  public class EntityPrinter
  {
    private string _oldBaseToStringCache;
    private string _toStringCache;
    private StringBuilder _toStringBuilder;

    private readonly INamedEntity _entity;

    public EntityPrinter(INamedEntity entity)
    {
      _entity = entity;
    }

    public string BuildToString()
    {
      if (_toStringCache != null)
        return _toStringCache;

      _toStringBuilder ??= new StringBuilder();

      _toStringBuilder.Length = 0;

      IComponent[] components = _entity.GetComponents();

      if (components.Length == 0)
        return "No components";

      string entityName = _entity.EntityName(components);

      entityName = entityName.Replace("Component", "");

      _toStringBuilder.Append($"{entityName}");
      
      _toStringCache = _toStringBuilder.ToString();

      _oldBaseToStringCache = _entity.BaseToString();

      return _toStringCache;
    }

    public void InvalidateCache()
    {
      if (_oldBaseToStringCache != _entity.BaseToString())
        _toStringCache = null;
    }
  }
}