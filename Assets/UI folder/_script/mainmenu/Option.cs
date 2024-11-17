using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    //also need to set change tutorial stuff
    public Animator animator;//for the main menu
    public AudioSource audioMainMenu;
    public GameObject option; 
    public Button back; 

    
    void Start()
    {     
        back.onClick.AddListener(CloseTutorial);
    }

    private void CloseTutorial()
    {
        if (option != null)
        {
            animator.enabled = true;
            audioMainMenu.enabled = true;
            playClick();
            option.SetActive(false); 
        }
    }
    private void playClick()
    {
        Audio.Instance.PlaySFX(AudioSFXUI.Instance.UIHoverAndClick);
    }
}
