using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal_display : MonoBehaviour
{
    public GameObject[] panelsToShow;
    public GameObject[] panelsToHide;
    public GameObject Page1;
    public GameObject Page2;

    // Method to show specific panels
    public void ShowPanels()
    {
        foreach (GameObject panel in panelsToShow)
        {
            panel.SetActive(true);
        }
    }

    // Method to hide specific panels
    public void HidePanels()
    {
        foreach (GameObject panel in panelsToHide)
        {
            panel.SetActive(false);
        }
    }

    public void playerPressedE()
    {
        Page1.SetActive(true);
        Page2.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
