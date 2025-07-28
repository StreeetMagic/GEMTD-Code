using HighlightPlus;
using UnityEngine;

namespace Game.Highlight
{
    public class HighlightedListenerView : EntityDependant, IHighlightedListener, IHighlightedRemovedListener
    {
        [SerializeField]
        private HighlightEffect _highlightEffect;

        private void Start()
        {
            Entity.AddHighlightedListener(this);
            Entity.AddHighlightedRemovedListener(this);
            _highlightEffect.highlighted = Entity.isHighlighted;
        }

        public void OnHighlighted(GameEntity entity)
        {
            _highlightEffect.highlighted = entity.isHighlighted;
        }

        public void OnHighlightedRemoved(GameEntity entity)
        {
            _highlightEffect.highlighted = entity.isHighlighted; 
        }
    }
}