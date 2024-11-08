using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Item item;
    public bool isPlayerInRange = false;
    private InventoryController inventoryController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        item = GetComponent<Item>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning(" No spriteRenderer?");
        }
        if (item == null)
        {
            Debug.LogWarning("Item script not found?");
        }    
    }

    private void Start()
    {
        inventoryController = GameObject.Find("Journal_Canvas")?.GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!inventoryController.IsInventoryFull())
            {
                inventoryController.AddItem(item.ItemData);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full.");
            }
        }
    }

    public void Initialize(ItemData data)
    {
        if (data != null)
        {
            item.SetItemData(data);
            spriteRenderer.sprite = data.droppedSprite;
            gameObject.name = data.itemName;
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
