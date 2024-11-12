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
    public Image tutorialImage;
    public Sprite[] gamePic;


    //tutorial button
    public Button[] btn;
    public GameObject[] description;

    
    void Start()
    {
        health();

        btn[0].onClick.AddListener(health);
        btn[1].onClick.AddListener(flashlight);
        btn[2].onClick.AddListener(objective);
        btn[3].onClick.AddListener(journal);
        btn[4].onClick.AddListener(craftnFuse);

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
    public void health()
    {
        resetInteractable();
        btn[0].interactable = false;
        description[0].gameObject.SetActive(true);

    }

    private void flashlight()
    {
        resetInteractable();
        btn[1].interactable = false;
        description[1].gameObject.SetActive(true);
    }

    private void objective()
    {
        resetInteractable();
        btn[2].interactable = false;
        description[2].gameObject.SetActive(true);
    }

    private void journal()
    {
        resetInteractable();
        btn[3].interactable = false;
        description[3].gameObject.SetActive(true);
    }

    private void craftnFuse()
    {
        resetInteractable();
        btn[4].interactable = false;
        description[4].gameObject.SetActive(true);
    }

    private void resetInteractable()
    {
        for(int i =0;i<5;i++)
        {
            btn[i].interactable = true;
            description[i].gameObject.SetActive(false);
        }
    }
}
