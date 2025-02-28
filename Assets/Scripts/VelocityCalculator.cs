using UnityEngine;

public class VelocityCalculator
{
    private readonly Camera _mainCamera;
    private readonly CircleCollider2D _rangeTriggerCollider;
    private readonly float _force;

    public VelocityCalculator(Camera camera, CircleCollider2D rangeTriggerCollider, float skullForce)
    {
        _mainCamera = camera;
        _rangeTriggerCollider = rangeTriggerCollider;
        _force = skullForce;
    }

    public Vector3 CalculateThrowVelocity(Transform transform)
    {
        var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        var borderCenter = _rangeTriggerCollider.transform.position;

        var direction = mousePosition - borderCenter;
        var distance = direction.magnitude;

        var velocity = -direction.normalized * _force;

        if (distance > _rangeTriggerCollider.radius)
        {
            direction = direction.normalized * (_rangeTriggerCollider.radius);
            transform.position = borderCenter + direction;

            velocity *= _rangeTriggerCollider.radius;
        }
        else
        {
            transform.position = mousePosition;

            velocity *= distance;
        }

        return velocity;
    }
}