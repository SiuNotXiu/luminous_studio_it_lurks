using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
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
        if (newItemData == null)
        {
            Debug.LogError("Trying to add null ItemData to CraftingSlot.");
            return;
        }
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
        //store tempitem
        ItemData tempItemData = this.itemData;

        //clear crafting slot
        ClearSlot();

        // return store item
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
}
