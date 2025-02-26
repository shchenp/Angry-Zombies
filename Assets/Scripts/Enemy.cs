using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField] 
    private AudioClip _deathSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Напишите логику уничтожения зомби тут
    }

    private void Die()
    {
        // Создаем эффект "взрыв" на месте убитого зомби.
        CreateExplosion();
        // ПРоигрываем звук смерти зомби.
        PlayDeathSound();
        // Разрушаем объект зомби.
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
}