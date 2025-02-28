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
    private AudioSource _rubberSound;
    
    [SerializeField]
    private AudioSource _throwSound;

    [SerializeField] 
    private float _force;

    [SerializeField] 
    private float _minVelocity;
    
    private Camera _mainCamera;
    private Vector3 _startPosition;
    private bool _isDragging;
    private bool _isFlying;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (_isFlying)
        {
            if (_rigidbody.velocity.magnitude < _minVelocity)
            {
                ResetSkull();
            }
            
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hitInfo.collider == _skullCollider)
            {
                _isDragging = true;
                OnDragging?.Invoke(_isDragging);
            }
            
            PlayRubberSound();
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
            _isDragging = false;
            _isFlying = true;
            OnDragging?.Invoke(_isDragging);
            
            _rigidbody.gravityScale = 1;


            var direction = transform.position - _rangeTriggerCollider.transform.position;
            var velocity = -direction.normalized * (direction.magnitude * _force);
            _rigidbody.velocity = velocity;

            _trajectoryRenderer.HideTrajectory();
            
            PlayThrowSound();
        }
    }

    private void ResetSkull()
    {
        _isFlying = false;
        transform.position = _startPosition;
        _rigidbody.rotation = 0;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0;
        _rigidbody.gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(GlobalConstants.OUT_OF_MAP_TAG))
        {
            ResetSkull();
        }
    }

    private void PlayRubberSound()
    {
        _rubberSound.Play();
    }

    private void PlayThrowSound()
    {
        _throwSound.Play();
    }
}