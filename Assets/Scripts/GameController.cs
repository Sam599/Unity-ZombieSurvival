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
    public Text enemiesKilledCounter;
    private Text timeSurvivedText, highTimeSurvivedText, highScoreText;
    public int zombieLimit, bossLimit, maxZombieGenerateInverval, minZombieGenerateInverval, zombieSpawnRadius, distanceFromPlayerToSpawn, wave, waveMultiplier, bossWave;
    private int zombiesKillCounter = 0;
    public bool stopZombieGenerateInLimit;
    public bool isGameProgressionActive;
    public bool spawnZombieBoss;
    public bool generatorReady;
    public int numZombieGenerated;
    public int numZombieAlive;
    public int numBossAlive;

    void Start()
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

    public void UpdateLifeBar(int healthPoints)
    {
        lifeBar.value = healthPoints;
        //Debug.Log(lifeBar.value);
    }

    public void IncreaseKillCounter()
    {
        zombiesKillCounter++;
        enemiesKilledCounter.text = "x " + zombiesKillCounter.ToString("D3");
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
            stopZombieGenerateInLimit = true;
            gameProgressMgr.enabled = true;
            gameProgressMgr.CalculateZombieWave(false);
        }
        generatorReady = true;
    }

    public void PlayerDied()
    {
        Time.timeScale = 0;
        float survivedTime = Time.timeSinceLevelLoad;
        float highScore = PlayerPrefs.GetFloat("HighScore");
        if (survivedTime > highScore)
        {
            PlayerPrefs.SetFloat("HighScore", survivedTime);
            highScoreText.text = "NOVO RECORDE!";
            highScoreText.color = Color.red;
            highTimeSurvivedText.text = FormatSurvivedTimeToString(survivedTime);
            highTimeSurvivedText.color = Color.red;
        }
        else
        {
            highTimeSurvivedText.text = FormatSurvivedTimeToString(highScore);
        }

        timeSurvivedText.text = FormatSurvivedTimeToString(survivedTime);
        enemiesKilledCounter.gameObject.SetActive(false);
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

    string FormatSurvivedTimeToString(float seconds)
    {
        int secAlive = (int)seconds % 60;
        int minAlive = (int)seconds / 60;
        int hourAlive = minAlive / 60;
        string timeSurvivedString = string.Format("{0}:{1}:{2}", hourAlive.ToString("D2"), minAlive.ToString("D2"), secAlive.ToString("D2"));

        return timeSurvivedString;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void RestartScore()
    {
        PlayerPrefs.SetFloat("HighScore", 0);
        highScoreText.text = "RECORDE RESETADO!";
        highScoreText.color = Color.blue;
        highTimeSurvivedText.text = "00:00:00";
        highTimeSurvivedText.color = Color.blue;
    }
}
