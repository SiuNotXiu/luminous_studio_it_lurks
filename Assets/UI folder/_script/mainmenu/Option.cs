using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    //also need to set change tutorial stuff
    public Animator animator;//for the main menu
    public GameObject option; 
    public Button back; 

    
    void Start()
    {     
        back.onClick.AddListener(CloseTutorial);
        animator.enabled = false;
    }

    private void CloseTutorial()
    {
        if (option != null)
        {
            animator.enabled = true;
            option.SetActive(false); 
        }
    }
}
