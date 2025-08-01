//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherGameLoopStateEnumListener;

    public static Entitas.IMatcher<GameEntity> GameLoopStateEnumListener {
        get {
            if (_matcherGameLoopStateEnumListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.GameLoopStateEnumListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGameLoopStateEnumListener = matcher;
            }

            return _matcherGameLoopStateEnumListener;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public void AddGameLoopStateEnumListener(IGameLoopStateEnumListener value) {
        var listeners = hasGameLoopStateEnumListener
            ? gameLoopStateEnumListener.value
            : new System.Collections.Generic.List<IGameLoopStateEnumListener>();
        listeners.Add(value);
        ReplaceGameLoopStateEnumListener(listeners);
    }

    public void RemoveGameLoopStateEnumListener(IGameLoopStateEnumListener value, bool removeComponentWhenEmpty = true) {
        var listeners = gameLoopStateEnumListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveGameLoopStateEnumListener();
        } else {
            ReplaceGameLoopStateEnumListener(listeners);
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

    public GameLoopStateEnumListenerComponent gameLoopStateEnumListener { get { return (GameLoopStateEnumListenerComponent)GetComponent(GameComponentsLookup.GameLoopStateEnumListener); } }
    public bool hasGameLoopStateEnumListener { get { return HasComponent(GameComponentsLookup.GameLoopStateEnumListener); } }

    public GameEntity AddGameLoopStateEnumListener(System.Collections.Generic.List<IGameLoopStateEnumListener> newValue) {
        var index = GameComponentsLookup.GameLoopStateEnumListener;
        var component = (GameLoopStateEnumListenerComponent)CreateComponent(index, typeof(GameLoopStateEnumListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
        return this;
    }

    public GameEntity ReplaceGameLoopStateEnumListener(System.Collections.Generic.List<IGameLoopStateEnumListener> newValue) {
        var index = GameComponentsLookup.GameLoopStateEnumListener;
        var component = (GameLoopStateEnumListenerComponent)CreateComponent(index, typeof(GameLoopStateEnumListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
        return this;
    }

    public GameEntity RemoveGameLoopStateEnumListener() {
        RemoveComponent(GameComponentsLookup.GameLoopStateEnumListener);
        return this;
    }
}
