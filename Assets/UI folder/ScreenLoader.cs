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
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        p1 = GameObject.Find("1st appear");
        p2 = FindInactiveObjectByName("2nd appear");

        if(skipAlert)
        {
            p1.SetActive(false);
            p2.SetActive(true);
        }

    }
    public void ButtonCall()
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
        else
        {
            //wait
            yield return new WaitForSeconds(transitionTime);
        }

        transition.SetTrigger("End");
        if (screen == "Premise")
        {
            Audio.Instance.SetBackgroundMusic(AudioSFXEnvironment.Instance.StoryBriefAmbience);

        }
        

        if (IsGameScene)
        {
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
