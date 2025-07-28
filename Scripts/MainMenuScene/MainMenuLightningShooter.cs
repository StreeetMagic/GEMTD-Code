using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Services.AudioServices;
using Services.AudioServices.Sounds;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;
using Zenject;

namespace MirzaBeig.LightningVFX
{
    public class MainMenuLightningShooter : MonoBehaviour
    {
        [FormerlySerializedAs("fxPrefab")] public GameObject FXPrefab;

        [FormerlySerializedAs("mainLight")] [Space]
        public Light MainLight;

        [FormerlySerializedAs("mainLightDimIntensity")] [Space]
        public float MainLightDimIntensity = 0.35f;

        [FormerlySerializedAs("fullDimLightningCount")] public int FullDimLightningCount = 5;

        [FormerlySerializedAs("mainLightIntensityLerpSpeed")] [Space]
        public float MainLightIntensityLerpSpeed = 1.0f;

        [SerializeField] private List<MeshRenderer> _grayEyes = new();
        [SerializeField] private List<MeshRenderer> _coloredEyes = new();

        private readonly List<GameObject> _spawnedLightningList = new List<GameObject>();
        private Camera _camera;
        private float _mainLightStartIntensity;
        private bool _isFirstLightningSpawn;
        private AudioService _audioService;

        [Inject]
        public void Construct(AudioService audioService)
        {
            _audioService = audioService;
        }

        private void Start()
        {
            _camera = Camera.main;
            _mainLightStartIntensity = MainLight.intensity;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
    
            if (!Mouse.current.leftButton.wasPressedThisFrame)
                return;
    
            var mousePosition = Mouse.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(mousePosition);
            var hits = new RaycastHit[5];
            Physics.RaycastNonAlloc(ray, hits,20, ~0, QueryTriggerInteraction.Ignore);
    
            RaycastHit validHit = default;
            bool foundValidHit = false;
    
            foreach (var hit in hits)
            {
                if (!hit.collider)
                    continue;
                
                if (!hit.collider.gameObject)
                    continue;
                
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UICollider"))
                    continue;
                
                validHit = hit;
                foundValidHit = true;
                break;
            }
    
            if (!foundValidHit)
                return;
    
            SpawnVisualFX(validHit);

            SpawnFirstLightning();
        }

        private void SpawnVisualFX(RaycastHit validHit)
        {
            var lightning = Instantiate(FXPrefab, validHit.point, Quaternion.identity);
            _audioService.Play(SoundEnum.LightningBolt, false);
            _spawnedLightningList.Add(lightning);
        }

        private void SpawnFirstLightning()
        {
            if (_isFirstLightningSpawn)
                return;
            
            foreach (var grayEye in _grayEyes)
            {
                grayEye.gameObject.SetActive(false);
            }
                
            foreach (var coloredEye in _coloredEyes)
            {
                coloredEye.gameObject.SetActive(true);
            }
                
            _isFirstLightningSpawn = true;
        }

        private void LateUpdate()
        {
            _spawnedLightningList.RemoveAll(x => x == null);

            float normalizedLightningCount = _spawnedLightningList.Count / (float)FullDimLightningCount;
            float mainLightTargetIntensity = Mathf.Lerp(_mainLightStartIntensity, MainLightDimIntensity, normalizedLightningCount);

            MainLight.intensity = Mathf.Lerp(MainLight.intensity, mainLightTargetIntensity, Time.deltaTime * MainLightIntensityLerpSpeed);
        }
    }
}