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
    private SkullAudioPlayer _audioPlayer;

    [SerializeField] 
    private float _force;

    [SerializeField] 
    private float _minVelocity;
    
    private Camera _mainCamera;
    private Vector3 _startPosition;
    private VelocityCalculator _velocityCalculator;
    private bool _isDragging;
    private bool _isFlying;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _startPosition = transform.position;

        _velocityCalculator = new VelocityCalculator(_mainCamera, _rangeTriggerCollider, _force);
    }

    private void Update()
    {
        // Полет снаряда
        if (_isFlying)
        {
            if (_rigidbody.velocity.magnitude < _minVelocity)
            {
                ResetSkull();
            }
            
            return;
        }
        
        // Начало прицеливания
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hitInfo.collider == _skullCollider)
            {
                _isDragging = true;
                OnDragging?.Invoke(_isDragging);
            }
            
            _audioPlayer.PlayRubberSound();
        }
        
        // Прицеливание
        if (_isDragging)
        {
            var velocity = _velocityCalculator.CalculateThrowVelocity(transform);

            _trajectoryRenderer.DrawTrajectory(transform.position, velocity);
        }
        
        // Начало полета
        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _isDragging = false;
            _isFlying = true;
            OnDragging?.Invoke(_isDragging);
            
            _rigidbody.gravityScale = 1;


           _rigidbody.velocity = _velocityCalculator.CalculateThrowVelocity(transform);

           _trajectoryRenderer.HideTrajectory();
            
           _audioPlayer.PlayThrowSound();
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
}