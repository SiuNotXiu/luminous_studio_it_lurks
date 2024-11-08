using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestController : MonoBehaviour
{
    public bool isOpen;
    public bool isInRange;

    public Animator animator;

    //chest recorgnise player


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

            isInRange = true;
            InventoryController.chest_detect = gameObject.GetComponent<ChestController>();
            InventoryController.chestIn = gameObject.GetComponent<CampInsideItem>();
            InventoryController.chestOut = gameObject.GetComponent<ChestInventory>();
            Debug.Log("Player is in the chest range of > " + gameObject.name);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = !isInRange;
            InventoryController.chest_detect = gameObject.GetComponent<ChestController>();
            Debug.Log("Player is not in the chest range");
        }
    }



    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Campfire loot is open");
            animator.SetBool("isOpen", isOpen);
        }

    }
}
