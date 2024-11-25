using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController; 
    [SerializeField] private string gateID; 

    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) 
        {
            TryUnlockGate();
        }
    }

    private void TryUnlockGate()
    {
        ItemData keyItemData = inventoryController.GetKeyDataForGate(gateID);

        if (keyItemData != null)
        {
            Debug.Log($"Gate {gateID} unlocked!");
            inventoryController.RemoveItemFromPlayerInventory(keyItemData); 
            Destroy(gameObject); 
        }
        else
        {
            Debug.Log("You need the correct key to unlock this gate!");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true; 
            Debug.Log($"Player entered range of gate {gateID}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false; 
            Debug.Log($"Player exited range of gate {gateID}");
        }
    }
}
