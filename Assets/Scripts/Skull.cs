using System;
using UnityEngine;

public class Skull : MonoBehaviour
{
    public Action<bool> OnDragging;
    
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private CircleCollider2D _skullCollider;

    [SerializeField]
    private CircleCollider2D _rangeTriggerCollider;
    
    [SerializeField] 
    private TrajectoryRenderer _trajectoryRenderer;

    [SerializeField] 
    private float _force;
    
    private Camera _mainCamera;
    private bool _isDragging;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hitInfo.collider == _skullCollider)
            {
                //todo Отрефакторить эту часть
                
                _isDragging = true;
                OnDragging?.Invoke(_isDragging);
            }
        }
        
        if (_isDragging)
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
            
            _trajectoryRenderer.DrawTrajectory(transform.position, velocity);
        }
        
        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            //todo Отрефакторить эту часть
            
            _isDragging = false;
            OnDragging?.Invoke(_isDragging);
            
            _rigidbody.gravityScale = 1;


            var direction = transform.position - _rangeTriggerCollider.transform.position;
            var velocity = -direction.normalized * (direction.magnitude * _force);
            _rigidbody.velocity = velocity;

            _trajectoryRenderer.HideTrajectory();
        }
    }
}