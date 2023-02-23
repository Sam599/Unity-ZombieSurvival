using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
    GameObject player;
    GameObject[] boss;
    [SerializeField] private Text bossSpawnedText;
    [SerializeField] private Text waveText;
    [SerializeField] private Text waveNumber;
    [SerializeField] private Text zombiePowerText;
    private CharactersStatus characterStatus;
    private bool waveAnnounced;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        boss = GameObject.FindGameObjectsWithTag("Boss");
        for (int i = 0; i < boss.Length; i++)
        {
            //Debug.Log("Found Boss");
            Slider bossLifeBar = boss[i].GetComponent<BossController>().bossLifeBar;
            characterStatus = boss[i].GetComponent<CharactersStatus>();
            if (bossLifeBar.value != characterStatus.currentHealth || bossLifeBar.maxValue != characterStatus.maxHealthPoints) {
                UpdateLifeBar(bossLifeBar);
            }
        }
    }
    public void UpdateLifeBar(Slider lifeBar)
    {
        lifeBar.maxValue = characterStatus.maxHealthPoints;
        lifeBar.value = characterStatus.currentHealth;
        float lifeRemaning = (float)characterStatus.currentHealth / characterStatus.maxHealthPoints;
        lifeBar.fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, lifeRemaning);
        //Debug.Log(string.Format("Life Bar Updated, {0}, {1}, {2}", lifeBar.maxValue, lifeBar.value, lifeRemaning));
    }

    public void NextWaveAnnoucement(int wave, float annoucementTime)
    {
        waveNumber.text = wave.ToString();
        StartCoroutine(FadeOutText(waveText, annoucementTime, 0));
        StartCoroutine(FadeOutText(waveNumber, annoucementTime, 0));
    }

    public void ShowZombiePowerText()
    {
        StartCoroutine(FadeOutText(zombiePowerText, 4, 1));
    }

    public void BossHasAppeared()
    {
        StartCoroutine(FadeOutText(bossSpawnedText, 3, 0));
    }

    IEnumerator FadeOutText(Text text, float fadeTime, float beginTime)
    {
        yield return new WaitForSeconds(beginTime);
        text.gameObject.SetActive(true);
        float timePassed = 0;
        Color originalColor = text.color;
        Color bossTextColor = text.color;
        while (timePassed <= 1) {
            timePassed += Time.deltaTime / fadeTime;
            bossTextColor.a = Mathf.Lerp(1, 0, timePassed);
            text.color = bossTextColor;
            yield return null;
        }
        text.gameObject.SetActive(false);
        text.color = originalColor;
    }
}
