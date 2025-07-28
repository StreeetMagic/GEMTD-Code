using UnityEngine;

namespace Services.Cameras
{
    public class CameraProvider : ICameraProvider
    {
        public Camera Camera { get; set; }
    }
}