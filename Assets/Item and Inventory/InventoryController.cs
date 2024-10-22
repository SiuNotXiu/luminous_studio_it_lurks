using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemData[] itemDataArray; // for resultslot (crafting)
    private Dictionary<string, ItemData> itemDataDict = new Dictionary<string, ItemData>();

    // UI
    [SerializeField] private Journal_display journal_display;
    [SerializeField] private Button_display button_display;
    [SerializeField] private ChestController chest_detect;
    public GameObject Journal;
    public ItemSlot[] itemSlot = new ItemSlot[6];
    public CraftingSlot[] craftingSlots = new CraftingSlot[2];
    public ResultSlot resultSlot;
    private bool JournalOpen = true;

    //specific page of the journal will open first
    public GameObject Page1;
    public GameObject Page2;

    // temp items (moved from craftingslot)
    private List<ItemData> tempItems = new List<ItemData>();

    // Store reference to the dropdown menu GameObject
    private GameObject dropdownMenuInstance;

    private void Start()
    {
        InitializeItemDictionary();
    }

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
        else if (Input.GetKeyDown(KeyCode.E) && JournalOpen && chest_detect.isInRange)
        {
            OpenJournal();
            Page1.SetActive(true);
            Page2.SetActive(false);

        }
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

        TryCrafting();

        // Clear crafting slots and move items back to tempItems if crafting is incomplete
        foreach (CraftingSlot slot in craftingSlots)
        {
            if (slot.HasItem())
            {
                tempItems.Add(slot.RemoveItem());
            }
        }

        ReturnTempItemsToInventory();
    }

    public bool IsJournalOpen()
    {
        return JournalOpen;
    }

    public void AddItem(ItemData itemData)
    {
        if (IsInventoryFull())
        {
            return;
        }
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(itemData);
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

    public bool AddItemToCraftingSlot(ItemData itemData)
    {
        foreach (CraftingSlot slot in craftingSlots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItem(itemData);
                return true;
            }
        }
        Debug.Log("No empty crafting slot available.");
        return false;
    }

    public void OnCraftingSlotUpdated()
    {
        if (craftingSlots[0].HasItem() && craftingSlots[1].HasItem())
        {
            TryCrafting();
        }
        else
        {
            // If both items are not present, clear the result slot
            resultSlot.ClearSlot();
        }
    }

    public void TryCrafting()
    {
        if (craftingSlots[0].HasItem() && craftingSlots[1].HasItem())
        {
            resultSlot.CheckAndShowCraftingResult(craftingSlots[0], craftingSlots[1]);
        }
    }

    public void AddCraftedItemToInventory(ItemData itemData)
    {
        if (!IsInventoryFull())
        {
            AddItem(itemData);
            resultSlot.ClearSlot();
        }
    }

    private void InitializeItemDictionary()
    {
        foreach (var itemData in itemDataArray)
        {
            if (itemData != null)
            {
                itemDataDict[itemData.itemName] = itemData;
            }
        }
    }

    public ItemData GetItemData(string itemName)
    {
        if (itemDataDict.ContainsKey(itemName))
        {
            return itemDataDict[itemName];
        }
        Debug.LogWarning("ItemData not found for: " + itemName);
        return null;
    }

    public void StoreTempItem(ItemData itemData)
    {
        tempItems.Add(itemData);
    }

    public void ReturnTempItemsToInventory()
    {
        foreach (ItemData item in tempItems)
        {
            AddItem(item);
        }
        tempItems.Clear();
    }

}


