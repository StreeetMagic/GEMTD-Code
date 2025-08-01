//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherView;

    public static Entitas.IMatcher<GameEntity> View {
        get {
            if (_matcherView == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.View);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherView = matcher;
            }

            return _matcherView;
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

    public Game.CommonComponents.ViewComponent view { get { return (Game.CommonComponents.ViewComponent)GetComponent(GameComponentsLookup.View); } }
    public Game.IGameEntityView View { get { return view.Value; } }
    public bool hasView { get { return HasComponent(GameComponentsLookup.View); } }

    public GameEntity AddView(Game.IGameEntityView newValue) {
        var index = GameComponentsLookup.View;
        var component = (Game.CommonComponents.ViewComponent)CreateComponent(index, typeof(Game.CommonComponents.ViewComponent));
        component.Value = newValue;
        AddComponent(index, component);
        return this;
    }

    public GameEntity ReplaceView(Game.IGameEntityView newValue) {
        var index = GameComponentsLookup.View;
        var component = (Game.CommonComponents.ViewComponent)CreateComponent(index, typeof(Game.CommonComponents.ViewComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
        return this;
    }

    public GameEntity RemoveView() {
        RemoveComponent(GameComponentsLookup.View);
        return this;
    }
}
