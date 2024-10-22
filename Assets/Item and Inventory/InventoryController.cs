using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // UI
    [SerializeField] private Journal_display journal_display;
    [SerializeField] private Button_display button_display;
    [SerializeField] private ChestController chest_detect;
    public GameObject Journal;
    public ItemSlot[] itemSlot = new ItemSlot[6];
    private bool JournalOpen = true;

    //specific page of the journal will open first
    public GameObject Page1;
    public GameObject Page2;

    // Store reference to the dropdown menu GameObject
    private GameObject dropdownMenuInstance;

    // Update is called once per frame
    public void Update()
    {
        // Open Journal
        if (Input.GetButtonDown("Journal") && JournalOpen)
        {
            OpenJournal();
            Page1.SetActive(false);
            Page2.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.E) && JournalOpen && chest_detect.isInRange)
        {
            OpenJournal();
            Page1.SetActive(true);
            Page2.SetActive(false);

        }
        // Close Journal with Escape or the same Journal button
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Journal")) && !JournalOpen)
        {
            CloseJournal();
            // Destroy dropdown menu instance if it exists
            if (dropdownMenuInstance != null)
            {
                Destroy(dropdownMenuInstance);
                dropdownMenuInstance = null;
            }
        }
    }

    private void OpenJournal()
    {
        Debug.Log("Opening Journal");
        JournalOpen = false;
        Journal.SetActive(true);
        journal_display.Show();
        journal_display.ShowPanels();
        button_display.Show();
        button_display.ShowPanels();
    }

    private void CloseJournal()
    {
        Debug.Log("Closing Journal");
        JournalOpen = true;
        button_display.HidePanels();
        journal_display.HidePanels();
        journal_display.Hide();
        button_display.Hide();
        Journal.SetActive(false);
    }

    public bool IsJournalOpen()
    {
        return JournalOpen;
    }

    public void AddItem(string itemName, string itemTag, Sprite itemSprite)
    {
        if (IsInventoryFull())
        {
            return;
        }
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(itemName, itemTag, itemSprite);
                return;
            }
        }
    }

    public bool IsInventoryFull()
    {
        foreach (var slot in itemSlot)
        {
            if (!slot.isFull)
            {
                return false; 
            }
        }
        return true;
    }

    public bool ArePanelsOpen()
    {
        return journal_display.isActiveAndEnabled || button_display.isActiveAndEnabled;
    }

    // Function to set the dropdown menu instance
    public void SetDropdownMenuInstance(GameObject dropdown)
    {
        dropdownMenuInstance = dropdown;
    }
}
