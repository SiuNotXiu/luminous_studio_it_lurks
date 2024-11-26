using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class EndingSetup : MonoBehaviour
{
    public Button Back;
    public GameObject[] EndingPic;

    void Start()
    {
        ClearBGM();
        Back.onClick.AddListener(BackToMenu);
        StartCoroutine(TimeAppear());
    }

    private IEnumerator TimeAppear()
    {

        foreach (var pic in EndingPic)
        {
            pic.SetActive(false);
        }

        for (int i = 0; i < EndingPic.Length; i++)
        {
            EndingPic[i].SetActive(true);
            if (i > 0)
            {
                EndingPic[i - 1].SetActive(false);
            }
            yield return new WaitForSeconds(7.5f); 
        }

        if (EndingPic.Length > 0)
        {
            EndingPic[EndingPic.Length - 1].SetActive(true); 
        }

        if (Back != null)
        {
            Back.gameObject.SetActive(true);
        }
    }

    private void BackToMenu()
    {
        if(ScreenLoader.Instance ==null)
        {
            SceneManager.LoadScene("1st Scene");
        }
        else
        {
            StartCoroutine(ScreenLoader.Instance.LoadLevel("1st Scene", true));
        }
    }

    #region Sound
    private void BGM()
    {
        if(Audio.Instance != null)
        Audio.Instance.SetBackgroundMusic(AudioSFXEnvironment.Instance.Ambience);
    }

    private void ClearBGM()
    {
        if (Audio.Instance != null)
            Audio.Instance.SetBackgroundMusic(null);
    }


    #endregion

}
