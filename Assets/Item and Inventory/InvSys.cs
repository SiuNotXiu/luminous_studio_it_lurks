using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<Item> items = new List<Item>(); 
    public int inventorySize = 6;

    public UI_Journal_Page2 inventoryUI;
    public UI_Journal_Page3 inventoryUI2;

    public bool AddItem(Item newItem)
    {
        if (items.Count < inventorySize)
        {
            items.Add(newItem);
            Debug.Log("Added " + newItem.itemName + " to inventory.");

            // Update UI
            inventoryUI.AddNewItem(newItem);

            return true;
        }
        else
        {
            Debug.Log("Inventory is full");
            return false;
        }
    }

    public void RemoveItem(Item itemToRemove)
    {
        if (items.Contains(itemToRemove))
        {
            items.Remove(itemToRemove);
            Debug.Log("Removed " + itemToRemove.itemName + " from inventory.");
        }
    }

    public void UseItem(Item itemToUse)
    {
        itemToUse.UseItem();
    }

    public void DropItem(Item itemToDrop)
    {
        itemToDrop.DropItem();
        RemoveItem(itemToDrop);
    }

    public void DiscardItem(Item itemToDiscard)
    {
        itemToDiscard.DiscardItem();
        RemoveItem(itemToDiscard);
    }
}