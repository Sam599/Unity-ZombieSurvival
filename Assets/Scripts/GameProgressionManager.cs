using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressionManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameController gameController;
    int bossWaveCounter = 1;
    int zombieLimit;

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
            gameController.numZombieGenerated = 0;
        }
        //Debug.Log("gameController is null? " + (gameController == null));
        int newZombieLimit = gameController.waveMultiplier * gameController.wave * zombieLimit;
        CheckForBossWave();
        gameController.zombieLimit = newZombieLimit;

        //Debug.Log(string.Format
        //   ("New Wave, Checking Variables: newZombieLimit: {0}, bossWaveCounter: {1}, bossWave: {2}", newZombieLimit, bossWaveCounter, gameController.bossWave));
    }

    void CheckForBossWave()
    {
        if (bossWaveCounter == gameController.bossWave)
        {
            bossWaveCounter = 0;
            gameController.spawnZombieBoss = true;
        }
    }
}
