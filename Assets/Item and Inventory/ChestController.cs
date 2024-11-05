using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public CampSlot[] chestSlots = new CampSlot[9]; 
    private Dictionary<string, ItemData> chestItemDict = new Dictionary<string, ItemData>();

    [SerializeField] private InventoryController playerInventory; 

    private void Start()
    {
        InitializeChestSlots();
    }

    
    private void InitializeChestSlots()
    {
        foreach (var slot in chestSlots)
        {
            slot.ClearSlot(); 
        }
    }

    public bool AddItemToChest(ItemData itemData)
    {
        foreach (var slot in chestSlots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItemToSlot(itemData);
                chestItemDict[itemData.itemName] = itemData;
                return true;
            }
        }
        Debug.Log("Chest is full. Cannot add item.");
        return false;
    }

    public bool RemoveItemFromChest(string itemName)
    {
        foreach (var slot in chestSlots)
        {
            if (slot.isFull && slot.itemData.itemName == itemName)
            {
                slot.ClearSlot();
                chestItemDict.Remove(itemName);
                return true;
            }
        }
        Debug.LogWarning("Item not found in chest.");
        return false;
    }

    public bool TransferItemToChest(string itemName)
    {
        ItemData item = playerInventory.GetItemData(itemName);
        if (item != null && AddItemToChest(item))
        {
         //   playerInventory.RemoveItem(itemName);
            Debug.Log("Transferred item to chest: " + itemName);
            return true;
        }
        return false;
    }

    // Transfer item from chest to player inventory
    public bool TransferItemToInventory(string itemName)
    {
        if (RemoveItemFromChest(itemName) && !playerInventory.IsInventoryFull())
        {
            ItemData item = chestItemDict[itemName];
            playerInventory.AddItem(item);
            Debug.Log("Transferred item to player inventory: " + itemName);
            return true;
        }
        Debug.LogWarning("Failed to transfer item to inventory.");
        return false;
    }
}
