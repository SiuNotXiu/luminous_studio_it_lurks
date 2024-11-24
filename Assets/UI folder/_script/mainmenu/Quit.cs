using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    
    public Animator animator;//for the main menu
    public AudioSource audioMainMenu;
    public GameObject quit; 
    public Button no;
    public Button yes;


    void Start()
    {     
        yes.onClick.AddListener(QuitGame);
        no.onClick.AddListener(CloseTab);
    }

    private void CloseTab()
    {
        if (quit != null)
        {
            animator.enabled = true;
            audioMainMenu.enabled = true;
            playClick();
            quit.SetActive(false);
        }
    }
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    private void playClick()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.UIHoverAndClick, Audio.Instance.SFXSource);
    }
}
