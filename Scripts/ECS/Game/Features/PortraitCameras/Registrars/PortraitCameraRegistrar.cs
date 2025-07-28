using Game.Registrars;
using UnityEngine;

namespace Game.PortraitCameras
{
    public class PortraitCameraRegistrar : EntityComponentRegistrar
    {
        public override void RegisterComponents()
        {
            Entity.isPortraitCamera = true;
            Entity.AddWorldPosition(transform.position);
            Entity.AddScale(Vector3.one);
        }

        public override void UnregisterComponents()
        {
            Entity.isPortraitCamera = false; 
            Entity.RemoveWorldPosition();
            Entity.RemoveScale();
        }
    }
}