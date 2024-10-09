using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemPickup : MonoBehaviour
{ 
    public Item item ;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BackpackInventory playerBackpackInventory = other.GetComponent<BackpackInventory>();
            if (playerBackpackInventory != null && other.GetComponent<BackpackInventory>().backpackInventorySpace.Count >= other.GetComponent<BackpackInventory>().backpackInventorySize)
            {
                Debug.Log(item.itemName + " cannot be picked up as inventory is full!");

            }
            if (playerBackpackInventory != null && other.GetComponent<BackpackInventory>().backpackInventorySpace.Count < other.GetComponent<BackpackInventory>().backpackInventorySize)
            {
                playerBackpackInventory.AddItem(item);
                Destroy(gameObject);
            }
        }
    }
}
