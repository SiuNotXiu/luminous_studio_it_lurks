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
            ApplyPerkEffects();
        }
        else if (!isBulbSlot && newItemData.isBatteryCompatible)
        {
            itemData = newItemData;
            isFull = true;
            itemImage.sprite = newItemData.itemSprite;
            ApplyPerkEffects();
        }
        else
        {
            Debug.Log("Item is not compatible with this slot.");
        }
    }

    public void ClearSlot()
    {
        RemovePerkEffects();
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
            if (inventoryController != null && !inventoryController.IsInventoryFull())
            {
                inventoryController.AddItem(itemData); 
                ClearSlot();
            }
            else
            {
                Debug.Log("Cannot add item. Player inventory is full.");
            }
        }
    }

    private void ApplyPerkEffects()
    {
        if (itemData == null) return;

        switch (itemData.itemName)
        {
            case "1300 mAh Casing":
                
                break;

            case "20k Lumen Bulb":
                
                break;

            default:
                Debug.LogWarning("No effect yet: " + itemData.itemName);
                break;
        }
    }

    private void RemovePerkEffects()
    {
        if (itemData == null) return;

        switch (itemData.itemName)
        {
            case "1300 mAh Casing":
                
                break;

            case "20k Lumen Bulb":
                
                break;

            default:
                Debug.LogWarning("No effect yet: " + itemData.itemName);
                break;
        }
    }
}
