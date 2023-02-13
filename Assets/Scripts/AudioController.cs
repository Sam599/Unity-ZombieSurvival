using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioController instance;
    public AudioClip shotSound;
    public AudioClip zombieDeathSound;
    public AudioClip playerHitSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    public void PlayShotSound()
    {
       audioSource.PlayOneShot(shotSound);
    }

    public void PlayZombieDeathSound()
    {
        audioSource.PlayOneShot(zombieDeathSound);
    }

    public void PlayPlayerHitSound()
    {
        audioSource.PlayOneShot(playerHitSound);
    }

}
