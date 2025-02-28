using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] 
    private AudioClip _impactSound;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(GlobalConstants.SKULL_TAG))
        {
            AudioSource.PlayClipAtPoint(_impactSound, transform.position);
        }
    }
}