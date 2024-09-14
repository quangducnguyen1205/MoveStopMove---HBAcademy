using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip attackClip;
    public AudioClip killBotClip;
    public AudioClip playerDieClip;
    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClip);
    }

    public void PlayKillBotSound()
    {
        audioSource.PlayOneShot(killBotClip);
    }

    public void PlayPlayerDieSound()
    {
        audioSource.PlayOneShot(playerDieClip);
    }
}
