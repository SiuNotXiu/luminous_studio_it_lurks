using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //UI
    [SerializeField] private Journal_display journal_display;
    [SerializeField] private Button_display button_display;
    public GameObject Journal;
    public ItemSlot[] itemSlot;
    private bool JournalOpen= true;

    public event Action OnJournalClosed; // Event to notify when journal is closed

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
        OnJournalClosed?.Invoke(); // Notify subscribers that the journal is closed
    }

    public bool IsJournalOpen()
    {
        return JournalOpen;
    }


    public void AddItem(string itemName, Sprite itemSprite)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, itemSprite);
                return;
            }
        }
    }



    public bool ArePanelsOpen()
    {
        return journal_display.isActiveAndEnabled || button_display.isActiveAndEnabled;
    }
}


