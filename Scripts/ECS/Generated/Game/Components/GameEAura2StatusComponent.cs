//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherEAura2Status;

    public static Entitas.IMatcher<GameEntity> EAura2Status {
        get {
            if (_matcherEAura2Status == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.EAura2Status);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherEAura2Status = matcher;
            }

            return _matcherEAura2Status;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly Game.Battle.EAura2StatusComponent eAura2StatusComponent = new Game.Battle.EAura2StatusComponent();

    public bool isEAura2Status {
        get { return HasComponent(GameComponentsLookup.EAura2Status); }
        set {
            if (value != isEAura2Status) {
                var index = GameComponentsLookup.EAura2Status;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : eAura2StatusComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}
