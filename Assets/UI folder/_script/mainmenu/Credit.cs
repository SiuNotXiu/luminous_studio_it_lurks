using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    //also need to set change tutorial stuff
    public Animator animator;//for the main menu
    public AudioSource audioMainMenu;
    public GameObject credit; 
    public Button back; 

    
    void Start()
    {     
        back.onClick.AddListener(CloseTutorial);
    }

    private void CloseTutorial()
    {
        if (credit != null)
        {
            animator.enabled = true;
            audioMainMenu.enabled = true;
            playClick();
            credit.SetActive(false); 
        }
    }
    private void playClick()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.UIHoverAndClick, Audio.Instance.SFXSource);
    }
}
