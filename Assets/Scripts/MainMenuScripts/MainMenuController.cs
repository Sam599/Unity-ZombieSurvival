using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject exitGameButton;
    [SerializeField] private GameObject gameTitle;
    [SerializeField] private GameObject mainMenuParent;
    [SerializeField] private GameObject skinSelectionParent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject player;
    [SerializeField] private Light SunLight;
    private Animator cameraAnimator;
    private bool readyToTransition;
    int playerChildIndex;
    int skinCount;
    int skinIndex;
    string _quitApplication;

    void Start()
    {
#if UNITY_STANDALONE
        exitGameButton.gameObject.SetActive(true);
        _quitApplication = "QuitGame";
        Debug.Log("Game Found! Application: " + _quitApplication);
#endif
#if UNITY_EDITOR
        exitGameButton.gameObject.SetActive(true);
        _quitApplication = "QuitEditor";
        Debug.Log("Editor Found! Application: " + _quitApplication);
#endif

        Time.timeScale = 1;
        cameraAnimator = Camera.main.GetComponent<Animator>();
        StartCoroutine(StartSunRotation());
        SearchForPlayerSkins();
        SetActiveSkin();
    }

    IEnumerator StartSunRotation()
    {
        Transform _dayLightReference = SunLight.transform;
        Transform _nightLightReference = SunLight.gameObject.transform.GetChild(0).transform;
        while (SunLight.transform.rotation != _nightLightReference.rotation)
        {
            SunLight.transform.rotation = Quaternion.Lerp(_dayLightReference.rotation, _nightLightReference.rotation, Time.deltaTime / 50);
            yield return null;
        }
        while (SunLight.transform.rotation != _dayLightReference.rotation)
        {
            SunLight.transform.rotation = Quaternion.Lerp(_nightLightReference.rotation, _dayLightReference.rotation, Time.deltaTime / 15);
            yield return null;
        }
        StartCoroutine(StartSunRotation());
    }

    public void StartGame()
    {
        StartCoroutine(ExecuteButtonFunction("Start"));
    }

    public void QuitGame()
    {
        StartCoroutine(ExecuteButtonFunction(_quitApplication));
    }

    public void ReturnMainMenu()
    {
        StartCoroutine(TransitionToMenu(skinSelectionParent, mainMenuParent, "skinSelection", false));
        PlayerPrefs.SetInt("SkinSelected", skinIndex);
    }

    public void SkinSelectionMenu()
    {
        StartCoroutine(TransitionToMenu(mainMenuParent, skinSelectionParent, "skinSelection", true));
    }

    public void NextSkin(bool forward)
    {
        if (skinIndex < skinCount && forward)
        {
            player.transform.GetChild(playerChildIndex).GetChild(skinIndex).gameObject.SetActive(false);
            skinIndex++;
            player.transform.GetChild(playerChildIndex).GetChild(skinIndex).gameObject.SetActive(true);
        }
        else if (skinIndex == 0 && !forward)
        {
            player.transform.GetChild(playerChildIndex).GetChild(skinIndex).gameObject.SetActive(false);
            skinIndex = skinCount;
            player.transform.GetChild(playerChildIndex).GetChild(skinIndex).gameObject.SetActive(true);
        }
        else if (skinIndex <= skinCount && !forward)
        {
            player.transform.GetChild(playerChildIndex).GetChild(skinIndex).gameObject.SetActive(false);
            skinIndex--;
            player.transform.GetChild(playerChildIndex).GetChild(skinIndex).gameObject.SetActive(true);
        }
        else if (skinIndex >= skinCount && forward)
        {
            player.transform.GetChild(playerChildIndex).GetChild(skinIndex).gameObject.SetActive(false);
            skinIndex = 0;
            player.transform.GetChild(playerChildIndex).GetChild(skinIndex).gameObject.SetActive(true);
        }
        //Debug.Log("Selected Skin: " + skinIndex + ", Skin Count: " + skinCount);
    }

    void SearchForPlayerSkins()
    {
        skinIndex = PlayerPrefs.GetInt("SkinSelected");

        for (int i = 0; i < player.transform.childCount; i++)
        {
            if (player.transform.GetChild(i).name.StartsWith("PlayerSkins"))
            {
                playerChildIndex = i;
                skinCount = player.transform.GetChild(i).childCount;
                skinCount--;
                //Debug.Log("Skins Parent Found! ParentIndex: " + playerChildIndex + ", Number of skins found: " + skinCount);
                break;
            }
        }
    }

    void SetActiveSkin()
    {
        player.transform.GetChild(playerChildIndex).GetChild(skinIndex).gameObject.SetActive(true);
    }

    IEnumerator TransitionToMenu(GameObject menuToDisable, GameObject menuToEnable, string camAnimationName, bool camAnimationValue)
    {
        menuToDisable.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        cameraAnimator.SetBool(camAnimationName, camAnimationValue);
        yield return new WaitForSeconds(0.5f);

        menuToEnable.SetActive(true);

    }

    IEnumerator ExecuteButtonFunction(string function)
    {
        yield return new WaitForSeconds(0.5f);

        if (function == "Start")
        {
            SceneManager.LoadScene("Game");
        }
        else if (function == "QuitGame")
        {
            Application.Quit();
        }
        else if (function == "QuitEditor")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }



}
