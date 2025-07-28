using System.Collections.Generic;
using UnityEngine;

namespace Game.Towers.Views
{
    public class TowerLevelListenerView : EntityDependant, ILevelListener
    {
        private readonly Dictionary<int, float> _levels = new()
        {
            { 1, .40f },
            { 2, .50f },
            { 3, .60f },
            { 4, .70f },
            { 5, .85f },
            { 6, 1 },
        };

        private void Start()
        {
            if (Entity == null)
                return;

            Entity.AddLevelListener(this);
            OnLevel(Entity, Entity.Level);
        }

        public void OnLevel(GameEntity entity, int value)
        {
            transform.localScale = Vector3.one * _levels[value]; 
        }
    }
}