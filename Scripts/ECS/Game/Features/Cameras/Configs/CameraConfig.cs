using System;
using System.Collections.Generic;
using Game.GameMainFeature;
using UnityEngine;

namespace Game.Cameras
{
    [CreateAssetMenu(menuName = "Configs/Camera", fileName = nameof(CameraConfig))]
    public class CameraConfig : ScriptableObject
    {
        [field: Header("Movement Speed")]
        [Range(0f, 20f)]
        [field: SerializeField]
        public float MoveSpeed { get; private set; } = 20f;

        [field: Header("Zoom Speed")]
        [Range(0f, 20f)]
        [field: SerializeField]
        public float ZoomSpeed { get; private set; } = 10f;

        [field: Header("Smoothness of the camera zoom")]
        [Range(2f, 7f)]
        [field: SerializeField]
        public float Damping { get; private set; } = 10f;

        [field: Header("Minimum approach distance")]
        [Range(-10f, 5f)]
        [field: SerializeField]
        public float MinHeight { get; private set; } = -5f;

        [field: Header("Maximum distance of separation")]
        [Range(0f, 10f)]
        [field: SerializeField]
        public float MaxHeight { get; private set; } = 5f;

        [field: Header("The speed of the camera movement through touching the edge of the screen with the mouse")]
        [Range(0f, 20f)]
        [field: SerializeField]
        public float StrafeSpeed { get; private set; } = 15f;

        [field: Header("Screen edge thickness before camera movement")]
        [Range(0f, 20f)]
        [field: SerializeField]
        public float EdgeThickness { get; private set; } = 20f;

        [field: Header("Отступ от границы death зоны камеры сверху")]
        [Range(0f, 5f)]
        [field: SerializeField]
        public float OffsetUp { get; private set; } = 2f;

        [field: Header("Отступ от границы death зоны камеры снизу")]
        [Range(2f, 7f)]
        [field: SerializeField]
        public float OffsetDown { get; private set; } = 4f;

        [SerializeField]
        private List<MapZoneEntry> zoneBounds = new();

        [Serializable]
        private class MapZoneEntry
        {
            public GameModeEnum zoneType;
            public CameraBoundsData bounds;
        }

        private Dictionary<GameModeEnum, CameraBoundsData> boundsDictionary;

        public void Initialize()
        {
            boundsDictionary = new Dictionary<GameModeEnum, CameraBoundsData>();

            foreach (var entry in zoneBounds)
                boundsDictionary[entry.zoneType] = entry.bounds;
        }

        public CameraBoundsData TryGetBounds(GameModeEnum zoneType)
        {
            if (!boundsDictionary.ContainsKey(zoneType))
                throw new Exception($"Bounds for {zoneType} was not found");
            
            return boundsDictionary[zoneType];
        }
    }
}