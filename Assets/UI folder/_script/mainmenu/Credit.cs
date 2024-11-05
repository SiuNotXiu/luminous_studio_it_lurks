using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    //also need to set change tutorial stuff
    public Animator animator;//for the main menu
    public GameObject credit; 
    public Button back; 

    
    void Start()
    {     
        back.onClick.AddListener(CloseTutorial);
        animator.enabled = false;
    }

    private void CloseTutorial()
    {
        if (credit != null)
        {
            animator.enabled = true;
            credit.SetActive(false); 
        }
    }
}
