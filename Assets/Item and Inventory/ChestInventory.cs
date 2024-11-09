using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ChestInventory : MonoBehaviour
{
    public ChestSlot[] chestSlots = new ChestSlot[9];
    public GameObject JournalP1;
    [SerializeField] public InventoryController playerInventory;
    [SerializeField] CampInsideItem chestItem;


    //store (from player to chest)
    public void StoreItemFromPlayer(ItemData itemData, int slotIndex)
    {
        for (int i = 0; i < chestSlots.Length; i++)
        {
            if (!chestSlots[i].isFull)
            {
                // Store item in the chest slot
                chestSlots[i].AddItem(itemData);
                playerInventory.RemoveItemFromPlayerInventory(itemData, slotIndex); // pass slot for correct inv removal
                Debug.Log("Item stored in chest slot " + i + ": " + itemData.itemName);
                return;
            }
        }
    }

    public void StoreItemFromChest(ItemData itemData, int slotIndex = -1)
    {
        if (slotIndex >= 0 && slotIndex < chestSlots.Length)
        {
            ChestSlot targetSlot = chestSlots[slotIndex];
            if (!targetSlot.isFull)
            {
                targetSlot.AddItem(itemData);
                Debug.Log("Item stored into chest slot no. " + targetSlot + ": " + itemData.itemName);
                return;
            }
            else
            {
                Debug.Log("Chest slot no. " + slotIndex + " is full, cannot store item.");
            }
        }
        else
        {
            Debug.Log("Old method, shouldn't be using");
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
    } 


    //retrieve (from chest to player)
    public void RetrieveItemToPlayer(ChestSlot chestSlot, int chestSlotIndex)
    {
        if (chestSlot.isFull && playerInventory.CanAddToPlayerInventory(chestSlot.itemData))
        {
            // Find the first empty slot in the player inventory
            for (int i = 0; i < playerInventory.itemSlot.Length; i++)
            {
                if (playerInventory.itemSlot[i].IsEmpty())
                {
                    // Move the item to the first available slot in the player inventory
                    playerInventory.itemSlot[i].AddItem(chestSlot.itemData);
                    chestSlot.ClearSlot();
                    Debug.Log("Item retrieved from chest to player inventory slot " + i + ": " + chestSlot.itemData.itemName);
                    return;
                }
            }
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
