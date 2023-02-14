using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartGameButton;
    public GameObject ExitGameButton;
    public AudioSource audioSource;
    public Light SunLight;
    string _quitApplication;

    void Start()
    {
        #if UNITY_STANDALONE
                ExitGameButton.gameObject.SetActive(true);
                _quitApplication = "QuitGame";
                Debug.Log("Game Found! Application: " + _quitApplication);
        #endif
        #if UNITY_EDITOR
        ExitGameButton.gameObject.SetActive(true);
               _quitApplication = "QuitEditor";
               Debug.Log("Editor Found! Application: " + _quitApplication);
        #endif


        StartCoroutine(StartSunRotation());
    }

    IEnumerator StartSunRotation()
    {
        //Quaternion _sunlightDefaultRot = SunLight.transform.RotateAround;
        //SunLight.transform.rotation = Quaternion.Lerp(SunLight.transform.rotation,);
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
