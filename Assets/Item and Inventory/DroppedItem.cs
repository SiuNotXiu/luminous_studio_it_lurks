using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Sprite droppedItemSprite;
    public Item item;
    public bool isPlayerInRange = false;
    private InventoryController inventoryController;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        item = GetComponent<Item>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (item == null)
        {
            Debug.LogWarning("Item script not found?");
        }
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Sprite of child missing?");
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
            droppedItemSprite = data.droppedSprite;
            gameObject.name = data.itemName;

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = droppedItemSprite;
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
