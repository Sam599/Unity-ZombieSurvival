using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioController instance;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip waveSound;
    [SerializeField] private AudioClip[] zombieDeathSound;
    [SerializeField] private AudioClip[] zombieSpottedSound;
    [SerializeField] private AudioClip[] zombieFallSound;
    [SerializeField] private AudioClip[] zombieGrabSound;
    [SerializeField] private AudioClip[] zombieAttackSound;
    [SerializeField] private AudioClip[] playerMaleHitSound;
    [SerializeField] private AudioClip[] playerFemaleHitSound;
    [SerializeField] private AudioClip[] playerMaleDeathSound;
    [SerializeField] private AudioClip[] playerFemaleDeathSound;
    [SerializeField] private AudioClip[] shellDropSound;
    [SerializeField] private AudioClip[] footStepSound;
    [SerializeField] private AudioClip[] bossSpawnSound;
    [SerializeField] private AudioClip[] bossAttackSound;
    [SerializeField] private AudioClip[] bossDeathSound;
    [SerializeField] private AudioClip[] bossHitSound;

    bool playerWalkingSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    public void GameOver()
    {
        audioSource.clip = gameOverMusic;
        audioSource.Play();
    }

    public void PlayShotSound()
    {
        audioSource.PlayOneShot(shotSound);
    }

    public void PlayWaveSound()
    {
        audioSource.PlayOneShot(waveSound,0.5f);
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

    public void PlayZombieGrabSound()
    {
        audioSource.PlayOneShot(RandomizeSound(zombieGrabSound), 0.5f);
    }

    public void PlayZombieAttackSound()
    {
        audioSource.PlayOneShot(RandomizeSound(zombieAttackSound), 0.5f);
    }

    public void PlayBossSpawnSound()
    {
        audioSource.PlayOneShot(RandomizeSound(bossSpawnSound));
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

    public void PlayPlayerHitSound(bool isMale)
    {
        if (isMale) audioSource.PlayOneShot(RandomizeSound(playerMaleHitSound));
        else audioSource.PlayOneShot(RandomizeSound(playerFemaleHitSound));
    }

    public void PlayPlayerDeathSound(bool isMale)
    {
        if (isMale) audioSource.PlayOneShot(RandomizeSound(playerMaleDeathSound));
        else audioSource.PlayOneShot(RandomizeSound(playerFemaleDeathSound));
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
