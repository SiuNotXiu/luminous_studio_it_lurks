using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController; 
    [SerializeField] private string gateID; 

    private bool isPlayerInGate = false;

    private void Update()
    {
        if (isPlayerInGate && Input.GetKeyDown(KeyCode.E)) 
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
            Debug.Log("Missing key bro?");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInGate = true;
            Debug.Log($"Player entered range of gate {gateID}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInGate = false;
            Debug.Log($"Player exited range of gate {gateID}");
        }
    }
}

