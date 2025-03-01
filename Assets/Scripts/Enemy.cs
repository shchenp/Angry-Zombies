using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public UnityEvent EnemyDied;
    
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField] 
    private AudioClip _deathSound;
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private float _maxVelocity;
    [SerializeField]
    private float _rotationThreshold;

    private float _startRotation;

    private void Start()
    {
        _startRotation = transform.eulerAngles.z;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GlobalConstants.SKULL_TAG))
        {
            Die();
            return;
        }

        if (IsFlipped())
        {
            Die();
            return;
        }

        if (_rigidbody.velocity.magnitude >= _maxVelocity)
        {
            Die();
        }
    }

    private bool IsFlipped()
    {
        var currentRotation = transform.eulerAngles.z;
        var rotationDifference = Mathf.Abs(Mathf.DeltaAngle(currentRotation, _startRotation));
        
        return rotationDifference > _rotationThreshold;
    }

    private void Die()
    {
        CreateExplosion();
        PlayDeathSound();
        
        EnemyDied?.Invoke();
        Destroy(gameObject);
    }

    private void PlayDeathSound()
    {
        AudioSource.PlayClipAtPoint(_deathSound, transform.position);
    }
    
    private void CreateExplosion()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        EnemyDied.RemoveAllListeners();
    }
}