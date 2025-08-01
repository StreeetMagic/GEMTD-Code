//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherDecreaseArmor;

    public static Entitas.IMatcher<GameEntity> DecreaseArmor {
        get {
            if (_matcherDecreaseArmor == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.DecreaseArmor);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherDecreaseArmor = matcher;
            }

            return _matcherDecreaseArmor;
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

    static readonly Game.Battle.DecreaseArmorComponent decreaseArmorComponent = new Game.Battle.DecreaseArmorComponent();

    public bool isDecreaseArmor {
        get { return HasComponent(GameComponentsLookup.DecreaseArmor); }
        set {
            if (value != isDecreaseArmor) {
                var index = GameComponentsLookup.DecreaseArmor;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : decreaseArmorComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}
