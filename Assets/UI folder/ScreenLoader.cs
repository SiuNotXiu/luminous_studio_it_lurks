using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenLoader : MonoBehaviour
{
    public static ScreenLoader Instance;
    public Animator transition;
    public float transitionTime = 3f;

    private GameObject p0;
    private GameObject p1;
    private GameObject p2;
    public static bool skipAlert = false;


    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }

    }


    private void Start()
    {
        p0 = FindInactiveObjectByName("School Logo");
        p1 = FindInactiveObjectByName("1st appear");
        p2 = FindInactiveObjectByName("2nd appear");

        if(skipAlert)
        {
            p0.SetActive(false);
            p1.SetActive(false);
            p2.SetActive(true);
            Audio.Instance.SetBackgroundMusic(AudioSFXEnvironment.Instance.Ambience);
        }

    }
    public void ButtonCall() //for 1st scene
    {
        StartCoroutine(FunctionCall());
    }

    public IEnumerator FunctionCall()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        p1.SetActive(false);
        p2.SetActive(true);
        transition.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);
        Audio.Instance.SetBackgroundMusic(AudioSFXEnvironment.Instance.Ambience);
       
    }

    public IEnumerator LoadLevel(string screen, bool IsGameScene, GameObject appear = null, GameObject disable = null)
    {
        //play animation
        transition.SetTrigger("Start");
        if(screen == "Premise")
        {
            yield return new WaitForSeconds(2f);
            Audio.Instance.SetBackgroundMusic(AudioSFXUI.Instance.StoryBriefPlay);
            yield return new WaitForSeconds(10.128f);
        }
        else if (screen == "-")
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            //designer like to fade in black and wait for xx sec setting
            yield return new WaitForSeconds(5f);

        }

        transition.SetTrigger("End");
        if (screen == "Premise")
        {
            Audio.Instance.SetBackgroundMusic(AudioSFXEnvironment.Instance.StoryBriefAmbience);

        }
        

        if (IsGameScene)
        {
            if (screen == "Main")
            {
                //HealthEffects.playerDead = false; already set at health effect script
                StartCoroutine(Audio.Instance.ForBGMSFX());
            }

            //Debug.Log("play scene");
            SceneManager.LoadScene(screen);
        }
        else
        {
            if (appear != null)
            {
                //Debug.Log("play active");
                appear.SetActive(true);
            }
            if (disable != null)
            {
                disable.SetActive(false);
            }

        }

        
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
