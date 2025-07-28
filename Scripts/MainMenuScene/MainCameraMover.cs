using DG.Tweening;
using UnityEngine;

public class MainCameraMover : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;
    public float Duration;
    public Ease Ease;

    private void Awake()
    {
        transform.position = StartPoint.position;
    }

    private void Start()
    {
        var tween = transform
                .DOMove(EndPoint.position, Duration)
                .SetEase(Ease)
            ;

        tween.OnComplete(() => tween.Kill());
    }
}