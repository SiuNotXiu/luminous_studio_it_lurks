using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenLoader : MonoBehaviour
{
    public static ScreenLoader Instance;
    public Animator transition;
    public float transitionTime = 3f;
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

    public IEnumerator LoadLevel(string screen, bool IsGameScene, GameObject appear = null, GameObject disable = null)
    {
        //play animation
        transition.SetTrigger("Start");
        if(screen == "Premise")
        {
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
}
