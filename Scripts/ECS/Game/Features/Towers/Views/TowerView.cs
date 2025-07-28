using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Towers.Views
{
    public class TowerView : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField]
        private float _animationDuration = 0.8f;

        [SerializeField]
        private float _maxScaleMultiplier = 1.3f;

        [SerializeField]
        private float _shakeStrength = 0.2f;

        [SerializeField]
        private float _rotationShake = 12f;

        [SerializeField]
        private float _jumpHeight = 0.5f;

        [SerializeField]
        private int _jumpCount = 1;

        private Vector3 _originalPosition;
        private Vector3 _originalRotation;
        private Vector3 _originalScale;

        private Sequence _spawnSequence;

        private void OnEnable()
        {
            StartCoroutine(PlaySpawnAnimation());
        }

        private void Start()
        {
            _originalPosition = transform.localPosition;
            _originalRotation = transform.localEulerAngles;
            _originalScale = transform.localScale;
        }

        private IEnumerator PlaySpawnAnimation()
        {
            yield return null;

            _spawnSequence?.Kill();

            transform.localScale = Vector3.zero;

            _spawnSequence = DOTween.Sequence();

            var targetMaxScale = Vector3.one * _maxScaleMultiplier;

            _spawnSequence.Append(
                transform
                    .DOScale(targetMaxScale, _animationDuration * 0.4f)
                    .SetEase(Ease.OutBack, 1.2f)
            );

            _spawnSequence.Append(
                transform
                    .DOScale(_originalScale, _animationDuration * 0.6f)
                    .SetEase(Ease.OutBounce)
            );

            _spawnSequence.Insert(
                _animationDuration * 0.2f,
                transform
                    .DOJump(_originalPosition, _jumpHeight, _jumpCount, _animationDuration * 0.6f)
                    .SetEase(Ease.OutQuad)
            );

            _spawnSequence.Join(
                transform.DOShakePosition(
                    _animationDuration * 0.7f,
                    new Vector3(_shakeStrength, 0f, _shakeStrength),
                    vibrato: 20,
                    randomness: 80f,
                    fadeOut: true
                )
            );

            _spawnSequence
                .Join(transform.DOShakeRotation(
                        _animationDuration * 0.7f,
                        _rotationShake,
                        vibrato: 18,
                        randomness: 80f,
                        fadeOut: true
                    )
                );

            _spawnSequence.OnComplete(() =>
            {
                transform.localPosition = _originalPosition;
                transform.localEulerAngles = _originalRotation;
                transform.localScale = _originalScale;
                _spawnSequence?.Kill();
            });
        }
    }
}