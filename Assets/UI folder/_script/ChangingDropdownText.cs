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
    public TextMeshProUGUI buttonText;

    public string CraftItemTag = "CraftItem";
    public string PerksItemTag = "PerksItem";

    //got bug
    public void Ava_Panels()
    {
        foreach (GameObject panel in AvailablePanel)
        {
            TextMeshProUGUI buttonText = Multi_function.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log("Button text component found: " + (buttonText != null));

            // Check if panel is active
            if (panel.name == "JournalP1_RightsideChest" && panel.activeSelf)
            {
                buttonText.text = Store;
                Debug.Log("Panel JournalP1 is active. Changing button text to: " + buttonText.text);
            }
            else if (panel.name == "JournalP2_RightsideCraft" && panel.activeSelf)
            {
                // Find all items within the panel (or as children) and check their tags
                bool textChanged = false;
                foreach (Transform child in panel.transform)
                {
                    Debug.Log("Checking child: " + child.name);

                    if (child.CompareTag(CraftItemTag))
                    {
                        buttonText.text = Craft;
                        Debug.Log("Craft item found. Changing button text to: " + buttonText.text);
                        textChanged = true;
                        break; // Stop once a craft item is found
                    }
                    else if (child.CompareTag(PerksItemTag))
                    {
                        buttonText.text = Perks;
                        Debug.Log("Perks item found. Changing button text to: " + buttonText.text);
                        textChanged = true;
                        break; // Stop once a perks item is found
                    }
                }
                if (!textChanged)
                {
                    Debug.Log("No matching items found in JournalP2.");
                }
            }
        }
    }

}
