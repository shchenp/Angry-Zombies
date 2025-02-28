using UnityEngine;

public class SkullAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource rubberSound;
    [SerializeField] private AudioSource throwSound;

    public void PlayRubberSound() => rubberSound.Play();
    public void PlayThrowSound() => throwSound.Play();
}