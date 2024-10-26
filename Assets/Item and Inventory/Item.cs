using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // Reference to the ItemData ScriptableObject

    public string ItemName => itemData != null ? itemData.itemName : "";
    public string ItemTag => itemData != null ? itemData.itemTag : "";
    public Sprite Sprite => itemData != null ? itemData.itemSprite : null;

    private InventoryController inventoryController;
    public bool isPlayerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GameObject.Find("Journal_Canvas").GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!inventoryController.IsInventoryFull() && itemData != null)
            {
                inventoryController.AddItem(itemData);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full bro stapt");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }
    
}
