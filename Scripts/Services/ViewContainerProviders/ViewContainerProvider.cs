using UnityEngine;

namespace Services.ViewContainerProviders
{
    public class ViewContainerProvider 
    {
        public Transform MapContainer { get; set; } 
        public Transform CommonContainer { get; set; }
         public Transform BlockContainer { get; set; }
         public Transform EnemyContainer { get; set; }
    }
}