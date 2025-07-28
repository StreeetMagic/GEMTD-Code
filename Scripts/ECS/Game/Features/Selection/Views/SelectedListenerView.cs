using HighlightPlus;
using UnityEngine;

namespace Game.Selection.Views
{
    public class SelectedListenerView : EntityDependant, ISelectedListener, ISelectedRemovedListener
    {
        [SerializeField]
        private HighlightEffect _highlightEffect;

        private void Start()
        {
            Entity.AddSelectedListener(this);
            Entity.AddSelectedRemovedListener(this);
            _highlightEffect.highlighted = Entity.isSelected;
        }

        public void OnSelected(GameEntity entity)
        {
            _highlightEffect.highlighted = entity.isSelected;
        }

        public void OnSelectedRemoved(GameEntity entity)
        {
            _highlightEffect.highlighted = entity.isSelected;
        }
    }
}