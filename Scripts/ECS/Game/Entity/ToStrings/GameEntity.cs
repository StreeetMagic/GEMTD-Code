using System;
using System.Linq;
using System.Text;
using Entitas;
using Game.Entity.ToStrings;
using Game.Extensions;
using UnityEngine;

public sealed partial class GameEntity : INamedEntity
{
  private EntityPrinter _printer;

  public override string ToString()
  {
    _printer ??= new EntityPrinter(this);

    _printer.InvalidateCache();

    return _printer.BuildToString();
  }

  public string EntityName(IComponent[] components)
  {
    try
    {
      if (components.Length == 1)
        return components[0].GetType().Name;

      string[] validComponentNames = ValidComponentNames.Value;

      foreach (IComponent component in components)
      {
        string componentName = component.GetType().Name;

        if (validComponentNames.Contains(componentName))
          return Print(componentName);
      }
    }
    catch (Exception exception)
    {
      Debug.LogError(exception.Message);
    }

    return components.First().GetType().Name;
  }

  private string Print(string name)
  {
    return new StringBuilder(name)
      .With(s => s.Append($" Id:{Id}"), when: hasId)
      .ToString();
  }

  public string BaseToString() =>
    base.ToString();
}