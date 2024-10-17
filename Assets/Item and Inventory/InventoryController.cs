using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // UI
    [SerializeField] private Journal_display journal_display;
    [SerializeField] private Button_display button_display;
    public GameObject Journal;
    public ItemSlot[] itemSlot;
    private bool JournalOpen = true;

    public int InventorySize => itemSlot.Length;

    // Store reference to the dropdown menu GameObject
    private GameObject dropdownMenuInstance;

    // Update is called once per frame
    public void Update()
    {
        // Open Journal
        if (Input.GetButtonDown("Journal") && JournalOpen)
        {
            OpenJournal();
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
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(itemName, itemTag, itemSprite);
                return;
            }
        }
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
