using UnityEngine;

public class TNT : MonoBehaviour
{
    [SerializeField] 
    private GameObject _explosionPrefab;
    [SerializeField] 
    private GameObject _explosionEffectPrefab;
    [SerializeField]
    private AudioSource _explosionSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(GlobalConstants.SKULL_TAG))
        {
            return;
        }

        CreateExplosion();
        PlayExplosionSound();
        Destroy(gameObject);
    }
    
    private void PlayExplosionSound()
    {
        _explosionSound.Play();
    }
    
    private void CreateExplosion()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
    }
}