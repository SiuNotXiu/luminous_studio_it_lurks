using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    private InventoryController inventoryController;

    //=====ITEM DATA=====//
    public ItemData itemData;
    public bool isFull;

    //=====ITEM SLOT=====//
    [SerializeField] private Image itemImage;

    // Start is called before the first frame update
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        //first check for itemimage
        if (itemImage == null)
        {
            itemImage = GetComponent<Image>();  // Ensure the image component is assigned
            if (itemImage == null)
            {
                Debug.LogError("itemImage is not assigned in CraftingSlot.");
            }
        }
    }

    public void AddItem(ItemData newItemData)
    {
        this.itemData = newItemData;
        this.isFull = true;
        this.itemImage.sprite = newItemData.itemSprite;
        Debug.Log("Item added to CraftingSlot: " + newItemData.itemName);

        inventoryController.OnCraftingSlotUpdated();
    }

    public bool IsEmpty()
    {
        return !isFull;
    }

    public ItemData RemoveItem()
    {
        ItemData tempItemData = this.itemData;
        ClearSlot();

        inventoryController.OnCraftingSlotUpdated();
        return tempItemData;
    }

    public void ClearSlot()
    {
        this.itemData = null;
        this.isFull = false;
        itemImage.sprite = null;
    }

    public bool HasItem()
    {
        return isFull;
    }

    public string GetItemName()
    {
        return itemData != null ? itemData.itemName : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If slot has an item, return it to the inventory
        if (isFull && itemData != null)
        {
            bool addedToInventory = inventoryController.AddCraftingSlotItemToInventory(itemData);
            if (addedToInventory)
            {
                Debug.Log("Item returned to inventory from CraftingSlot: " + itemData.itemName);
                RemoveItem(); // Clear slot only if item was successfully added to inventory
            }
            else
            {
                Debug.LogWarning("Inventory is full. Cannot return item to inventory.");
            }
        }
        else
        {
            Debug.LogWarning("CraftingSlot clicked, but it is empty.");
        }
    }
}
