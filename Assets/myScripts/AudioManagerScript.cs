using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{

    public AudioSource audioSource;

    public AudioClip phoneCall;

    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
