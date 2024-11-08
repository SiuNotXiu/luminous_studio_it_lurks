using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultSlot : MonoBehaviour, IPointerClickHandler
{
    private InventoryController inventoryController;
    private Dictionary<string, string> knownRecipes;

    //=====ITEM DATA=====//
    public ItemData resultItemData;
    public bool isFull = false;

    //=====ITEM SLOT=====//
    [SerializeField] private Image resultImage;

    public event Action<ResultSlot> OnClicked;

    private void Start()
    {
        InitializeRecipes();
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void InitializeRecipes()
    {
        knownRecipes = new Dictionary<string, string>
        {
            // example recipe: combine "ItemA" and "ItemB" to get "ResultItem"
            { "ginseng+yarrow", "Bushcraft Medicine" },
            { "yarrow+ginseng", "Bushcraft Medicine" }
        };
    }

    public void CheckAndShowCraftingResult(CraftingSlot slot1, CraftingSlot slot2)
    {
        if (slot1.IsEmpty() || slot2.IsEmpty())
        {
            ClearSlot();
            return;
        }

        string item1Name = slot1.GetItemName().ToLower();
        string item2Name = slot2.GetItemName().ToLower();
        string recipeKey = item1Name + "+" + item2Name;

        if (knownRecipes.ContainsKey(recipeKey))
        {
            string resultItemName = knownRecipes[recipeKey];
            ItemData resultItemData = inventoryController.GetItemData(resultItemName);

            if (resultItemData != null)
            {
                ShowResult(resultItemData);
                Debug.Log("New item created: " + resultItemData);
            }
            else
            {
                Debug.LogError("Result ItemData not found for recipe: " + recipeKey);
            }
        }
        else
        {
            // No valid recipe, clear the result slot
            Debug.LogWarning("No valid recipe found for: " + recipeKey);
            ClearSlot();
        }
    }

    public void ShowResult(ItemData itemData)
    {
        this.resultItemData = itemData;
        this.resultImage.sprite = itemData.itemSprite;
        resultImage.enabled = true;
        isFull = true;
    }

    public void ClearSlot()
    {
        this.resultItemData = null;
        this.resultImage.sprite = null;
        resultImage.enabled = false;
        isFull = false;
    }

    public bool IsFull()
    {
        return isFull;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isFull && resultItemData != null)
            {
                OnClicked?.Invoke(this);
                inventoryController.AddCraftedItemToInventory(resultItemData);

                foreach (CraftingSlot slot in inventoryController.craftingSlots)
                {
                    slot.ClearSlot();
                }

                ClearSlot();
            }
            else
            {
                Debug.LogWarning("ResultSlot clicked, but it is empty or invalid");
            }
        }
    }

    public void ResetResultOnInventoryClose()
    {
        ClearSlot();
    }
}
