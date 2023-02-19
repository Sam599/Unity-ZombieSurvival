using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressionManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameController gameController;
    int bossWaveCounter = 1;
    int zombieLimit;
    float increaseZombiePowerFlt = 0.2f;
    int waveZombiePowerCount = 0;

    void OnEnable()
    {
        gameController = GetComponent<GameController>();
        zombieLimit = gameController.zombieLimit;
    }

    public void CalculateZombieWave(bool waveEnded)
    {
        if (waveEnded)
        {
            gameController.wave++;
            bossWaveCounter++;
            waveZombiePowerCount++;
            gameController.numZombieGenerated = 0;
        }
        //Debug.Log("gameController is null? " + (gameController == null));
        int newZombieLimit = gameController.waveMultiplier * gameController.wave * zombieLimit;
        CheckForBossWave();
        CheckForZombiePowerIncrease();
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
            gameController.zombiePowerMultiplier += increaseZombiePowerFlt;
            Debug.Log("Zombie Power Increased!");
        }
    }
}
