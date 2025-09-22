using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{

    public AudioSource audioSource;

    public AudioClip phoneCall;
    public AudioClip button1;
    public AudioClip button2;
    public AudioClip button3;
    public AudioClip button4;
    public AudioClip button5;
    public AudioClip button6;
    public AudioClip button7;
    public AudioClip button8;
    public AudioClip button9;
    public AudioClip button0;
    public AudioClip redialButton;
    public AudioClip dialTone;
    public AudioClip wrongCall;

    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayLoop(AudioClip clip)
    {
        audioSource.clip = clip;      
        audioSource.loop = true;       
        audioSource.Play();            
    }

    public void StopLoop()
    {
        audioSource.loop = false;      
        audioSource.Stop();           
    }
}
