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
    private SkullState _currentState;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _startPosition = transform.position;

        _velocityCalculator = new VelocityCalculator(_mainCamera, _rangeTriggerCollider, _force);
        _currentState = SkullState.Idle;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case SkullState.Idle:
                Idle();
                break;
            case SkullState.Aiming:
                Aim();
                break;
            case SkullState.Flying:
                Fly();
                break;
        }
    }

    private void Fly()
    {
        if (_isDragging)
        {
            _isDragging = false;
            OnDragging?.Invoke(_isDragging);

            _rigidbody.gravityScale = 1;
            _rigidbody.velocity = _velocityCalculator.CalculateThrowVelocity(transform);

            _trajectoryRenderer.HideTrajectory();
            _audioPlayer.PlayThrowSound();
        }

        if (_rigidbody.velocity.magnitude < _minVelocity)
        {
            ResetSkull();
            _currentState = SkullState.Idle;
        }
    }

    private void Idle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hitInfo.collider == _skullCollider)
            {
                _currentState = SkullState.Aiming;
            }
        }
    }

    private void Aim()
    {
        if (!_isDragging)
        {
            _audioPlayer.PlayRubberSound();
            _isDragging = true;
            OnDragging?.Invoke(_isDragging);
        }
        else
        {
            var velocity = _velocityCalculator.CalculateThrowVelocity(transform);
            _trajectoryRenderer.DrawTrajectory(transform.position, velocity);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _currentState = SkullState.Flying;
        }
    }

    private void ResetSkull()
    {
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