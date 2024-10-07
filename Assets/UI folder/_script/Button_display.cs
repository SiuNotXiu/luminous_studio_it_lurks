using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_display : MonoBehaviour
{
    public GameObject[] panelsToHide;  // Array to store panels to hide
    public GameObject[] panelsToShow;  // Array to store panels to show

    // Method to show specific panels
    public void ShowPanels()
    {
        // Iterate over the panelsToShow array and activate each panel
        foreach (GameObject panel in panelsToShow)
        {
            panel.SetActive(true);
        }
    }

    // Method to hide specific panels
    public void HidePanels()
    {
        // Iterate over the panelsToHide array and deactivate each panel
        foreach (GameObject panel in panelsToHide)
        {
            panel.SetActive(false);
        }
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

