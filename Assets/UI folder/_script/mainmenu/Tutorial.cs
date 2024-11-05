using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    //also need to set change tutorial stuff
    public Animator animator;//for the main menu
    public GameObject tutorial; 
    public Button back; 

    
    void Start()
    {     
        back.onClick.AddListener(CloseTutorial);
        animator.enabled = false;
    }

    private void CloseTutorial()
    {
        if (tutorial != null)
        {
            animator.enabled = true;
            tutorial.SetActive(false); 
        }
    }
}
