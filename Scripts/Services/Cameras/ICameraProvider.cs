using UnityEngine;

namespace Services.Cameras
{
    public interface ICameraProvider
    {
        Camera Camera { get; set; }
    }
}