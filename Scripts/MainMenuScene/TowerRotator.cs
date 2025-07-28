using UnityEngine;

public class TowerRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;

    [Header("Vertical Rotation Limits (X Axis)")]
    [SerializeField] private float _minXAngle = -30f;
    [SerializeField] private float _maxXAngle = 45f;

    [Header("Horizontal Turn Limits (Y Axis)")]
    [Tooltip("Maximum allowed angle (in degrees) to turn from current forward direction")]
    [SerializeField] private float _maxTurnAngle = 100f;

    private void Update()
    {
        RotateTowardsCursor();
    }

    private void RotateTowardsCursor()
    {
        Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;
        
        Vector3 direction = hit.point - transform.position;

        if (!(direction.sqrMagnitude > 0.001f))
            return;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Vector3 clampedEuler = targetRotation.eulerAngles;
        clampedEuler.x = ClampAngle(clampedEuler.x, _minXAngle, _maxXAngle);
        targetRotation = Quaternion.Euler(clampedEuler);

        float angle = Vector3.Angle(transform.forward, direction);
        transform.rotation = angle <= _maxTurnAngle 
            ? Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime) 
            : Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime * _maxTurnAngle);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        angle = angle > 180 ? angle - 360 : angle;
        return Mathf.Clamp(angle, min, max);
    }
}