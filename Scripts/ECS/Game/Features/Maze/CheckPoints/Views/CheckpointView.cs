using System;
using TMPro;
using UnityEngine;

namespace Game.Maze.Views
{
    public class CheckpointNumberView : MonoBehaviour
    {
        public GameEntityView EntityView;
        public TextMeshProUGUI Text;

        private void Start()
        {
            var entity = EntityView.Entity;

            if (entity.isStartPoint)
                Text.text = "";

            if (entity.isFinishPoint)
                Text.text = "";

            if (entity.hasCheckPoint)
            {
                Text.text = entity.CheckPoint.ToString(); 
            }
        }
    }
}