using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            GateSFX();
            inventoryController.RemoveItemFromPlayerInventory(keyItemData);

            if (gateID == "4")
            {
                Debug.Log("game ending");
                SceneManager.LoadScene("Ending"); 
            }
            else
            {
                Destroy(gameObject); 
            }
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

    #region SoundEffect
    private void GateSFX()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.GateOpen, Audio.Instance.SFXSource);
        }
    }



    #endregion
}

