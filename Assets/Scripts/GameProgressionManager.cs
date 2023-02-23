using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameProgressionManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameController gameController;
    public UnityEvent onZombiePowerEvent;

    int bossWaveCounter = 1;
    int zombieLimit;
    int waveZombiePowerCount = 0;
    int powerUpCount = 0;
    int newZombieLimit;
    int waveCounter;

    void OnEnable()
    {
        gameController = GetComponent<GameController>();
        zombieLimit = gameController.zombieLimit;
        waveCounter = gameController.wave;
    }

    public void CalculateZombieWave(bool waveEnded)
    {
        if (waveEnded)
        {
            gameController.wave++;
            waveCounter++;
            bossWaveCounter++;
            waveZombiePowerCount++;
            gameController.numZombieGenerated = 0;
        }
        //Debug.Log("gameController is null? " + (gameController == null));
        CheckForBossWave();
        CheckForZombiePowerIncrease();
        newZombieLimit = gameController.waveMultiplier * waveCounter * zombieLimit;
        gameController.zombieLimit = newZombieLimit;

    }

    void CheckForBossWave()
    {
        if (bossWaveCounter == gameController.bossWave)
        {
            bossWaveCounter = 0;
            gameController.spawnZombieBoss = true;
        }
    }

    void CheckForZombiePowerIncrease()
    {
        if (waveZombiePowerCount == gameController.waveZombiePower)
        {
            waveZombiePowerCount = 0;
            waveCounter = 1;
            powerUpCount++;
            gameController.zombiePower = powerUpCount;
            onZombiePowerEvent.Invoke();
            //Debug.Log("Zombie Power Increased! PowerUpCount: " + gameController.zombiePower);
        }
    }
}
