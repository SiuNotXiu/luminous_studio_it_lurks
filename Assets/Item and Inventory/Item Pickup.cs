using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemPickup : MonoBehaviour
{ 
    public Item item ; // for SO
    private bool isPickedUp = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPickedUp) { return; }
        if (other.CompareTag("Player") && isPickedUp == false)
        {
            InventorySystem inventory = other.GetComponent<InventorySystem>();
            if (inventory != null)
            {
                if (inventory.AddItem(item))
                {
                    isPickedUp = true;
                    Destroy(gameObject);
                }
      

            }

        }
    }
    
}
