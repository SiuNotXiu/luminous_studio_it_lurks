using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //UI
    [SerializeField] private Journal_display journal_display;
    [SerializeField] private Button_display button_display;
    [SerializeField] private ItemSlot slotDropDown;
    public GameObject Journal;
    public ItemSlot[] itemSlot;
    private bool JournalOpen= true;


    private void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetButtonDown("Journal")&& JournalOpen )
        {
            Debug.Log("Trigger Tab1");
            JournalOpen = false;
            Journal.SetActive(true);
            journal_display.Show();
            journal_display.ShowPanels();

            button_display.Show();
            button_display.ShowPanels();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)&& !JournalOpen)
        {
            Debug.Log("Trigger Tab2");
            slotDropDown.HideDropdownMenu();
            button_display.HidePanels();
            journal_display.HidePanels();
            journal_display.Hide();
            button_display.Hide();
            Journal.SetActive(false);

            JournalOpen = true;

        }
        else if (Input.GetButtonDown("Journal") && !JournalOpen)
        {
            Debug.Log("Trigger Tab3");
            slotDropDown.HideDropdownMenu();
            button_display.HidePanels();
            journal_display.HidePanels();
            journal_display.Hide();
            button_display.Hide();
            Journal.SetActive(false);
            JournalOpen = true;

        }
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


