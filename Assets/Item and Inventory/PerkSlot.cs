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
    public bool batteryCaseCheck = false; //check battery in slot

    //=====ITEM SLOT=====//
    [SerializeField] private bool isBulbSlot; // true for Bulb - false for Battery
    [SerializeField] private Image itemImage;
    private readonly float filledAlpha = 1f;     // item added (opaque)
    private readonly float emptyAlpha = 0f;    // item remove (transparent)

    private bool effectApplied = false;

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
            SetItemImageAlpha(filledAlpha);
            ApplyPerkEffects();
        }
        else if (!isBulbSlot && newItemData.isBatteryCompatible)
        {
            itemData = newItemData;
            isFull = true;
            itemImage.sprite = newItemData.itemSprite;
            SetItemImageAlpha(filledAlpha);
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

        SetItemImageAlpha(emptyAlpha);
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
        if (itemData == null || effectApplied) return;

        switch (itemData.itemName)
        {
            case "1300 mAh Casing":

                #region equip 1300 mah casing
                //battery_bar_float.equip_1300_mah_casing();
                //TopdownMovement.equip_20k_lumen_bulb();
                #endregion

                effectApplied = true;
                break;

            case "20k Lumen Bulb":
                #region equip 20k lumen bulb
                flashlight_fov_wall_mask.view_distance = flashlight_fov_wall_mask.view_distance_initial * 2;
                battery_bar_float.equip_20k_lumen_bulb();
                #endregion
                effectApplied = true;
                break;

            default:
                Debug.LogWarning("No effect yet: " + itemData.itemName);
                break;
        }
    }

    private void RemovePerkEffects()
    {
        if (itemData == null || !effectApplied) return;

        switch (itemData.itemName)
        {
            case "1300 mAh Casing":
                #region remove 1300 mah casing
                //battery_bar_float.remove_1300_mah_casing();
                //TopdownMovement.remove_20k_lumen_bulb();
                #endregion
                effectApplied = false;
                break;

            case "20k Lumen Bulb":
                #region remove 20k lumen bulb
                flashlight_fov_wall_mask.view_distance = flashlight_fov_wall_mask.view_distance_initial;
                battery_bar_float.remove_20k_lumen_bulb();
                #endregion
                effectApplied = false;
                break;

            default:
                Debug.LogWarning("No effect yet: " + itemData.itemName);
                break;
        }
    }

    private void SetItemImageAlpha(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }
}
