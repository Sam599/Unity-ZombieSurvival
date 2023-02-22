using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.UIElements;

public class GameController : MonoBehaviour
{
    GameProgressionManager gameProgressMgr;
    InterfaceController interfaceController;
    public GameObject gameOverScreen;
    public Slider lifeBar;
    public Text enemiesAliveCounter;
    public Text waveNumber;
    private Text timeSurvivedText, highTimeSurvivedText, highScoreText;
    public int zombieLimit, bossLimit, maxZombieGenerateInverval, minZombieGenerateInverval, zombieSpawnRadius;
    public int distanceFromPlayerToSpawn, wave, waveMultiplier, bossWave, waveZombiePower;
    public int zombiePower = 0;
    public bool stopZombieGenerateInLimit;
    public bool isGameProgressionActive;
    public bool spawnZombieBoss;
    public bool generatorReady;
    public int numZombieGenerated;
    public int numZombieAlive;
    public int numBossAlive;

    void Awake()
    {
        Time.timeScale = 1;
        gameProgressMgr = GetComponent<GameProgressionManager>();
        interfaceController = GetComponent<InterfaceController>();
        generatorReady = false;
        SetGeneratorSettings();
        timeSurvivedText = gameOverScreen.transform.GetChild(1).gameObject.GetComponent<Text>(); // NOT PROUD
        highScoreText = gameOverScreen.transform.GetChild(2).gameObject.GetComponent<Text>(); // NOT PROUD
        highTimeSurvivedText = gameOverScreen.transform.GetChild(3).gameObject.GetComponent<Text>(); // NOT PROUD
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
        UpdateZombiesAliveCounter();
    }

    public void UpdateLifeBar(float healthPoints)
    {
        lifeBar.value = healthPoints;
        //Debug.Log(lifeBar.value);
    }

    public void UpdateWaveNumber()
    {
        waveNumber.text = wave.ToString("D2");
    }

    public void UpdateZombiesAliveCounter()
    {
        int numTotalZombieAlive = numZombieAlive + numBossAlive;
        enemiesAliveCounter.text = numTotalZombieAlive.ToString("D2");
    }

    public void EnemyKilled(string tag)
    {
        switch (tag)
        {
            case "Enemy":
                numZombieAlive--;
                if (numZombieAlive == 0 && isGameProgressionActive)
                {
                    gameProgressMgr.CalculateZombieWave(true);
                    UpdateWaveNumber();
                }
                break;
            case "Boss":
                numBossAlive--;
                break;
        }
    }

    void SetGeneratorSettings()
    {
        if (isGameProgressionActive)
        {
            wave = 1;
            waveMultiplier = 10;
            zombieLimit = 1;
            bossLimit = 1;
            maxZombieGenerateInverval = 0;
            minZombieGenerateInverval = 0;
            zombieSpawnRadius = 3;
            distanceFromPlayerToSpawn = 15;
            waveZombiePower = bossWave;
            stopZombieGenerateInLimit = true;
            gameProgressMgr.enabled = true;
            gameProgressMgr.CalculateZombieWave(false);
            UpdateWaveNumber();
        }
        generatorReady = true;
    }

    public void PlayerDied()
    {
        Time.timeScale = 0;
        int highScore = PlayerPrefs.GetInt("HighScore");
        if (wave > highScore)
        {
            PlayerPrefs.SetInt("HighScore", wave);
            highScoreText.text = "NOVO RECORDE!";
            highScoreText.color = Color.red;
            highTimeSurvivedText.text = "ONDA " + wave.ToString();
            highTimeSurvivedText.color = Color.red;
        }
        else
        {
            highTimeSurvivedText.text = "ONDA " + highScore.ToString();
        }

        timeSurvivedText.text = "ONDA " + wave.ToString("D2");
        enemiesAliveCounter.gameObject.SetActive(false);
        lifeBar.gameObject.SetActive(false);
        gameOverScreen.SetActive(true);
        //Debug.Log("Player Died!");
    }

    public void BossSpawned()
    {
        numBossAlive++;
        spawnZombieBoss = false;
        interfaceController.BossHasAppeared();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void RestartScore()
    {
        PlayerPrefs.SetInt("HighScore", wave);
        highScoreText.text = "RECORDE RESETADO!";
        highScoreText.color = Color.blue;
        highTimeSurvivedText.text = "ONDA " + wave.ToString("D2");
        highTimeSurvivedText.color = Color.blue;
    }
}
