using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemData[] itemDataArray; // for resultslot (crafting)
    private Dictionary<string, ItemData> itemDataDict = new Dictionary<string, ItemData>();

    // UI
    [SerializeField] private Journal_display journal_display;
    [SerializeField] private Button_display button_display;
    public static ChestController chest_detect;
    public GameObject Journal;
    public GameObject Description;

    // GAME SYSTEMS
    public ItemSlot[] itemSlot = new ItemSlot[6];
    public CraftingSlot[] craftingSlots = new CraftingSlot[2];
    public ResultSlot resultSlot;
    public PerkSlot bulbUpgradeSlot;
    public PerkSlot batteryUpgradeSlot;

    public static bool JournalOpen = true;
    private bool Special_Bool_For_Inventory = true;

    // temp items (moved from craftingslot)
    private List<ItemData> tempItems = new List<ItemData>();

    // Store reference to the dropdown menu GameObject
    private GameObject dropdownMenuInstance;

    public GameObject Page1;
    public GameObject Page2;
    public GameObject button_Pg1;
    public GameObject setting;
    public GameObject journal_p1n2;

    [Header("Switching Sprite")]
    public Image Boarder;
    public Sprite[] PageSprite;

    [Header("For chest")]
    [SerializeField] public static ChestInventory chestOut;
    [SerializeField] public static CampInsideItem chestIn;
    public static Item item;

    private void Start()
    {
        InitializeItemDictionary();
    }

    // Update is called once per frame
    public void Update()
    {
        // Open Journal
        // Switching is use to switch the boarder
        if (Input.GetButtonDown("Journal") && JournalOpen && !trigger_map_ui.Map_Is_Open)
        {
            Debug.Log("Cheking Journal>>>>>>>>>>>>>>" + JournalOpen);
            OpenJournal();
            Switching2();
            button_Pg1.SetActive(false);
            Page1.SetActive(false);
            Page2.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.E) && !trigger_map_ui.Map_Is_Open)
        {
            if(Item.isPlayerInRange)
            {
                //Debug.Log("In range>>>>>>>>: " + Item.isPlayerInRange);

                item.Item_Scrap_Check();
            }
            else if (JournalOpen && chest_detect.isInRange && !trigger_map_ui.Map_Is_Open)
            {
                chest_detect.OpenChest();
                //Debug.Log("is it gay?: " + chest_detect.isInRange);//true
                chestIn.AsssignToPlayerJournal();
                Special_Bool_For_Inventory = true;
                OpenJournal();
                Switching1();
                Page1.SetActive(true);
                Page2.SetActive(false);
            }
            else
            {
                return;
            }


        }
        else if(Input.GetKeyDown(KeyCode.Escape) && JournalOpen && !trigger_map_ui.Map_Is_Open)
        {
 
            OpenJournal();
            button_Pg1.SetActive(false);
            journal_p1n2.SetActive(false);
            setting.SetActive(true);

        }
        // Close Journal with Escape or the same Journal button or backspace
        else if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Journal")) && !JournalOpen)
        {
            Debug.Log("Cheking Journal>>>>>>>>>>>>>>" + JournalOpen);

            CloseJournal();
        }
    }


    private void OpenJournal()
    {
        if(player_database.is_flashlight_on)
        {
            player_database.is_flashlight_on = false;
            flashlightSFX();
        }
        ONFJournal();
        //chest problem here in chest in
        //Debug.Log("wwwwwwwww > " + chestIn);
        //Debug.Log("Opening Journal");
        JournalOpen = false;
        Journal.SetActive(true);
        journal_display.Show();
        journal_display.ShowPanels();
        button_display.Show();
        button_display.ShowPanels();
        
    }

    private void CloseJournal()
    {

        if(Special_Bool_For_Inventory)//for chest inventory
        {
            chestOut.ClosingChest();
            Special_Bool_For_Inventory= false;
        }

        if (dropdownMenuInstance != null)// Destroy dropdown menu instance if it exists
        {
            Destroy(dropdownMenuInstance);
            dropdownMenuInstance = null;
        }
        ONFJournal();
        Description.SetActive(false);
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

        foreach (var itemSlot in itemSlot)
        {
            itemSlot.ResetToDefaultSprite();
        }

        ReturnTempItemsToInventory();

        resultSlot.ResetResultOnInventoryClose();
    }

    // Function to set the dropdown menu instance
    public void SetDropdownMenuInstance(GameObject dropdown)
    {
        dropdownMenuInstance = dropdown;
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
        else if (!craftingSlots[0].HasItem() || !craftingSlots[1].HasItem())
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

    public bool AddCraftingSlotItemToInventory(ItemData itemData)
    {
        if (IsInventoryFull())
        {
            Debug.Log("Cannot add item. Inventory is full.");
            return false;
        }
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(itemData);
                Debug.Log("Item added to inventory slot: " + itemData.itemName);
                return true;
            }
        }
        Debug.Log("No empty slot found to add item.");
        return false;
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

    public void Switching1()
    {
        Boarder.sprite = PageSprite[0];


    }
    public void Switching2()
    {
        Boarder.sprite = PageSprite[1];
    }

    public void FuseItemToPerkSlot(ItemData itemData)
    {
        if (itemData.isBulbCompatible && bulbUpgradeSlot.IsEmpty())
        {
            bulbUpgradeSlot.FuseItem(itemData);
        }
        else if (itemData.isBatteryCompatible && batteryUpgradeSlot.IsEmpty())
        {
            batteryUpgradeSlot.FuseItem(itemData);
        }
        else
        {
            Debug.Log("No compatible perk slot or slot is full.");
        }
    }

    public void RemoveItemFromPlayerInventory(ItemData itemData, int slotIndex = -1)
    {
        if (slotIndex >= 0 && slotIndex < itemSlot.Length)
        {
            var slot = itemSlot[slotIndex];
            if (slot.isFull && slot.itemData == itemData)
            {
                slot.ClearSlot();
                Debug.Log("Removed item from inventory slot " + slotIndex + ": " + itemData.itemName);
            }
            else
            {
                Debug.Log("Slot " + slotIndex + " is empty or doesn't contain the specified item.");
            }
        }
        else
        {
            // just remove the item from any matching slot in the inventory
            foreach (var slot in itemSlot)
            {
                if (slot.isFull && slot.itemData == itemData)
                {
                    slot.ClearSlot();
                    Debug.Log("Removed item from inventory: " + itemData.itemName);
                    return;
                }
            }
            Debug.Log("Item not found in player inventory.");
        }
    }

    public bool CanAddToPlayerInventory(ItemData itemData)
    {
        foreach (var slot in itemSlot)
        {
            if (!slot.isFull)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItemToPlayerInventory(ItemData itemData)
    {
        foreach (var slot in itemSlot)
        {
            if (!slot.isFull)
            {
                slot.AddItem(itemData);
                return;
            }
        }
        Debug.Log("Player inventory is full.");
    }

    #region Sound effect
    private void ONFJournal()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.JournalOpenClose, Audio.Instance.SFXSource, 0.37f,0.7f);
        }
    }
    private void flashlightSFX()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXPlayerBehave.Instance.Flashlight, Audio.Instance.playerFlashlight);
        }
    }


    #endregion
}