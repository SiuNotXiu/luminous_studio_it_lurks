using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestController : MonoBehaviour
{
    public bool isOpen;
    public bool isInRange;
    [SerializeField] ItemSlot assignRange;

    public Animator animator;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is in the chest range");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = !isInRange;
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
