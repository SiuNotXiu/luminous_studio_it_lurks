using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenLoader : MonoBehaviour
{
    public static ScreenLoader Instance;
    public Animator transition;
    public float transitionTime = 1f;
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

        //wait
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("End");
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
