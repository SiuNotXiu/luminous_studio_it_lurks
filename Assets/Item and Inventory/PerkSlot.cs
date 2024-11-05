using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PerkSlot : MonoBehaviour, IPointerClickHandler
{
    private InventoryController inventoryController;

    //=====ITEM DATA=====//
    public ItemData itemData;
    public bool isFull;

    //=====ITEM SLOT=====//
    [SerializeField] private bool isBulbSlot; // true for Bulb - false for Battery
    [SerializeField] private Image itemImage;

    // Start is called before the first frame update
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        if (inventoryController == null)
        {
            Debug.LogError("InventoryController not found in the scene.");
        }
    }

    public void FuseItem(ItemData newItemData)
    {
        if (isBulbSlot && newItemData.isBulbCompatible)
        {
            itemData = newItemData;
            isFull = true;
            itemImage.sprite = newItemData.itemSprite;
        }
        else if (!isBulbSlot && newItemData.isBatteryCompatible)
        {
            itemData = newItemData;
            isFull = true;
            itemImage.sprite = newItemData.itemSprite;
        }
        else
        {
            Debug.Log("Item is not compatible with this slot.");
        }
    }

    public ItemData RemoveItem()
    {
        ItemData tempItemData = this.itemData;
        ClearSlot();
        return tempItemData;
    }

    public void ClearSlot()
    {
        this.itemData = null;
        this.isFull = false;
        itemImage.sprite = null;
    }

    public bool IsEmpty()
    {
        return !isFull;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFull && itemData != null)
        {
            // Check if the inventory is full before adding
            if (inventoryController != null && !inventoryController.IsInventoryFull())
            {
                inventoryController.AddItem(itemData); // Add item to the inventory
                ClearSlot(); // Clear the slot after moving item
            }
            else
            {
                Debug.Log("Cannot add item. Player inventory is full.");
            }
        }
    }
}
