using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangingDropdownText : MonoBehaviour
{
    public GameObject[] AvailablePanel; // 3 panels: p1, p2, leftside inventory
    public Button Multi_function; // Drag the button from the Inspector
    private string Store = "Store";
    private string Craft = "Craft";
    private string Perks = "Fuse";

    public string CraftItemTag = "CraftItem";
    public string PerksItemTag = "PerksItem";

    //got bug
    public void Ava_Panels()
    {
        foreach (GameObject panel in AvailablePanel)
        {
            TextMeshProUGUI buttonText = Multi_function.GetComponentInChildren<TextMeshProUGUI>();
            // Check if panel is active
            if (panel.name == "JournalP1_RightsideChest" && panel.activeSelf)
            {
                
                buttonText.text = Store;
                Debug.Log(Multi_function.GetComponentInChildren<TextMeshProUGUI>());
                Debug.Log(buttonText.text);
            }

            else if (panel.name == "JournalP2_RightsideCraft" && panel.activeSelf)
            {
                // Find all items within the panel (or as children) and check their tags
                foreach (Transform child in panel.transform)
                {
                    // If the child has a CraftItem tag
                    if (child.CompareTag(CraftItemTag))
                    {
                        buttonText.text = Craft;
                        break; // Stop once a craft item is found
                    }
                    // If the child has a PerksItem tag
                    else if (child.CompareTag(PerksItemTag))
                    {
                        buttonText.text = Perks;
                        break; // Stop once a perks item is found (reduce)
                    }
                }
            }
        }
    }
}
