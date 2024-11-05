using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_menu : MonoBehaviour
{
    public Button button_g;
    public Button button_t;
    public Button button_o;
    public Button button_c;
    public Button button_q;


    private void Start()
    {
        button_g.onClick.AddListener(GameOn);  //start game
        button_t.onClick.AddListener(Tutorial); //tutorial
        button_o.onClick.AddListener(Option); //option
        button_c.onClick.AddListener(Credit); //credit
        button_q.onClick.AddListener(Quit); //quit
    }

    public void GameOn()
    {
        button_g.interactable = true; //this need to set if the pop up ot active the button
    }

    public void Tutorial()
    {

    }

    public void Option()
    {

    }

    public void Credit()
    {

    }
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
