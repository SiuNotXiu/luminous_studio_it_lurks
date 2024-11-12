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
        SceneManager.LoadScene("1st Scene");
    }
}
