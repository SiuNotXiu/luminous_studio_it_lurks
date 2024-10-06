using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal_Switching_Button : MonoBehaviour
{
    // Variables to store the panels
    public GameObject panelToHide1;
    public GameObject panelToHide2;
    public GameObject panelToShow;

    // Method to switch panels
    public void SwitchPanels()
    {
        // Hide the first panel
        panelToHide1.SetActive(false);
        panelToHide2.SetActive(false);
        // Show the second panel
        panelToShow.SetActive(true);
    }
}
