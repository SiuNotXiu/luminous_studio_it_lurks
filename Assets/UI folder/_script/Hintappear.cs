using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hintappear : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    //public RectTransform hintpanel;      
    //public Vector3 offset = new Vector3(0, 1.5f, 0); // Adjust this offset to position above the head

    [SerializeField] private InventoryController inventoryController;
    public GameObject hint;

    bool interactible = false;

    private readonly HashSet<string> interactibleTags = new HashSet<string>
    {
        "CraftItem", "PerksItem", "Campsite", "ScrapPaper", "EasterEgg", "Gate"
    };
    void Start()
    {
        if (hint != null)
            hint.SetActive(false);
    }

    private void Update()
    {
        if (hint != null)
        {
            hint.SetActive(interactible); 

            //if (interactible)
                //Reposition(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactibleTags.Contains(collision.tag))
        {
            if (collision.CompareTag("Gate"))
            {
                if (PlayerHasAnyKey())
                {
                    interactible = true;
                }
                else
                {
                    interactible = false;
                }
            }
            else
            {
                interactible = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactibleTags.Contains(collision.tag))
        {
            //Debug.Log($"{gameObject.name} stopped interacting with {collision.gameObject.name}");
            interactible = false;
        }
    }


    private bool PlayerHasAnyKey()
    {
        foreach (ItemSlot slot in inventoryController.itemSlot)
        {
            if (slot.isFull && slot.itemData != null && slot.itemData.isKey)
            {
                return true;
                Debug.Log("Player have a key");
            }
        }
        return false;
    }
    #region deleted code
    /*    private void Reposition()
        {
            if (playerTransform == null || hintpanel == null) return;

            // Calculate the world position for the hint image
            Vector3 hintPosition = playerTransform.position + offset;

            // Convert the world position to screen space
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, hintPosition);

            // Set the hint panel position to the calculated screen point
            hintpanel.position = screenPoint;
        }*/
    #endregion
}
