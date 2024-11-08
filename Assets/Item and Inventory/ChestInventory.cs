using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : MonoBehaviour
{
    public ChestSlot[] chestSlots = new ChestSlot[9];
    public GameObject JournalP1;
    [SerializeField] public InventoryController playerInventory;
    [SerializeField] CampInsideItem chestItem;


    //store (from player to chest)
    public void StoreItemFromPlayer(ItemData itemData)
    {
        foreach (var slot in chestSlots)
        {
            if (!slot.isFull)
            {
                slot.AddItem(itemData);
                playerInventory.RemoveItemFromPlayerInventory(itemData);
                Debug.Log("Item stored in chest: " + itemData.itemName);
                return;
            }
        }
        Debug.Log("Chest is full, cannot store item.");
    }
    public void StoreItemFromChest(ItemData itemData)
    {
        foreach (var slot in chestSlots)
        {
            if (!slot.isFull)
            {
                slot.AddItem(itemData);
                
                Debug.Log("Item stored in chest: " + itemData.itemName);
                return;
            }
        }
        Debug.Log("Chest is full, cannot store item.");
    }


    //retrieve (from chest to player)
    public void RetrieveItemToPlayer(ChestSlot chestSlot)
    {
        if (chestSlot.isFull && playerInventory.CanAddToPlayerInventory(chestSlot.itemData))
        {
            playerInventory.AddItemToPlayerInventory(chestSlot.itemData);
            chestSlot.ClearSlot();
            Debug.Log("Item retrieved from chest: " + chestSlot.itemData.itemName);
        }
        else
        {
            Debug.Log("Cannot retrieve item, player inventory may be full.");
        }
    }

    public void ClosingChest()//active this at inventoryController
    {
        foreach (var slot in chestSlots)
        {
            if (slot != null) // Ensure slot exists and contains an item
            {
                // Retrieve item data from the slot
                ItemData itemData = slot.GetItemData();

                if (itemData != null)
                {
                    chestItem.TakeBackFromPlayerJournal(slot.GetItemData());
                    slot.ClearSlot();
                    continue;

                }
            }
            else
            {
                Debug.Log("Empty or invalid slot, skipping.");
            }
        }
    }

    public bool CanAddToChestInventory(ItemData itemdata)
    {
        foreach (var slot in chestSlots)
        {
            if (!slot.isFull)
            {
                return true;
            }
        }
        return false;
    }
}
