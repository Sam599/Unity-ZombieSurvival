using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioController instance;
    public AudioClip shotSound;
    public AudioClip[] zombieDeathSound;
    public AudioClip[] zombieSpottedSound;
    public AudioClip[] zombieFallSound;
    public AudioClip[] playerHitSound;
    public AudioClip[] shellDropSound;
    public AudioClip[] footStepSound;
    public AudioClip[] bossSpawnSound;
    public AudioClip[] bossAttackSound;
    public AudioClip[] bossDeathSound;
    public AudioClip[] bossHitSound;

    bool playerWalkingSound;

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
        audioSource.PlayOneShot(RandomizeSound(zombieDeathSound),0.5f);
    }

    public void PlayZombieSpottedSound()
    {
        audioSource.PlayOneShot(RandomizeSound(zombieSpottedSound),0.5f);
    }

    public void PlayZombieFallSound()
    {
        audioSource.PlayOneShot(RandomizeSound(zombieFallSound), 0.5f);
    }

    public void PlayBossSpawnSound()
    {
        audioSource.PlayOneShot(RandomizeSound(bossSpawnSound), 0.5f);
    }

    public void PlayBossAttackSound()
    {
        audioSource.PlayOneShot(RandomizeSound(bossAttackSound), 0.5f);
    }

    public void PlayBossDeathSound()
    {
        audioSource.PlayOneShot(RandomizeSound(bossDeathSound), 0.5f);
    }

    public void PlayBossHitSound()
    {
        audioSource.PlayOneShot(RandomizeSound(bossHitSound), 0.5f);
    }

    public void PlayPlayerHitSound()
    {
        audioSource.PlayOneShot(RandomizeSound(playerHitSound));
    }

    public void PlayShellDropSound()
    {
        audioSource.PlayOneShot(RandomizeSound(shellDropSound),0.3f);
    }

    public void PlayFootStepSound()
    {
        if (!playerWalkingSound)
        {
            AudioClip footStepSelect = RandomizeSound(footStepSound);
            audioSource.PlayOneShot(footStepSelect);
            StartCoroutine(WaitForSoundEnd(footStepSelect));
        }
    }

    public AudioClip RandomizeSound(AudioClip[] soundClips)
    {
        int numberOfSounds = soundClips.Length;
        int soundIndex = Random.Range(0, numberOfSounds);
        return soundClips[soundIndex];
    }

    public IEnumerator WaitForSoundEnd(AudioClip sound)
    {
        playerWalkingSound = true;
        yield return new WaitForSeconds(sound.length);
        playerWalkingSound = false;
    }

}
