using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToStart : MonoBehaviour
{
    public Button Quit;



    void Start()
    {
        Quit.onClick.AddListener(Back);
    }

    private void Back()
    {
        ResetBGM();
        InventoryController.JournalOpen = true;//true is close
        SceneManager.LoadScene("1st Scene");
    }

    #region Sound
    private void ResetBGM()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.SetBackgroundMusic(null);
        }
    }


    #endregion
}
