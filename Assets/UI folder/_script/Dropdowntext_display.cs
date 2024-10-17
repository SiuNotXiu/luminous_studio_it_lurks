using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropdowntext_display : MonoBehaviour
{
    // Display options for different contexts
    public GameObject[] StoreUndisplay;
    public GameObject[] CraftUndisplay;
    public GameObject[] PerksUndisplay;

    // References to context-checking GameObjects
    public GameObject Storing;
    public GameObject Craft;

    // Reference to the item data
    [SerializeField] private ItemSlot itemSlot;

    // Method to update the UI based on context and item tag
    public void UpdateDisplay()
    {
        HideAllPanels();

        if (Storing.activeSelf)
        {
            Debug.Log("Displaying Store Panels");
            StorePanels();
        }
        else if (Craft.activeSelf)
        {
            if (itemSlot.itemTag == "CraftItem")
            {
                Debug.Log("Displaying Craft Panels");
                CraftPanels();
            }
            else if (itemSlot.itemTag == "PerksItem")
            {
                Debug.Log("Displaying Perks Panels");
                PerksPanels();
            }
        }
    }

    // Method to hide all panel options
    private void HideAllPanels()
    {
        foreach (GameObject panel in StoreUndisplay)
        {
            panel.SetActive(false);
        }
        foreach (GameObject panel in CraftUndisplay)
        {
            panel.SetActive(false);
        }
        foreach (GameObject panel in PerksUndisplay)
        {
            panel.SetActive(false);
        }
    }

    // Method to display store panels
    public void StorePanels()
    {
        foreach (GameObject panel in StoreUndisplay)
        {
            panel.SetActive(true); 
        }
    }

    // Method to display crafting panels
    public void CraftPanels()
    {
        foreach (GameObject panel in CraftUndisplay)
        {
            panel.SetActive(true);  
        }
    }

    // Method to display perks panels
    public void PerksPanels()
    {
        foreach (GameObject panel in PerksUndisplay)
        {
            panel.SetActive(true); 
        }
    }
}
