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
    private readonly float filledAlpha = 1f;     // item added (opaque)
    private readonly float emptyAlpha = 0f;    // item remove (transparent)

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
            { "yarrow+ginseng", "Bushcraft Medicine" },
            { "stick+stick", "Large Stick" },
            { "large stick+large stick", "Tent Beam" },
            { "branch+branch", "Tent Flysheet" },
            { "tent beam+tent flysheet", "Makeshift Camp" },
            { "tent flysheet+tent beam", "Makeshift Camp" }
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

        SetItemImageAlpha(filledAlpha);
    }

    public void ClearSlot()
    {
        this.resultItemData = null;
        this.resultImage.sprite = null;
        resultImage.enabled = false;
        isFull = false;

        SetItemImageAlpha(emptyAlpha);
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
                playCrafting();
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

    private void SetItemImageAlpha(float alpha)
    {
        Color color = resultImage.color;
        color.a = alpha;
        resultImage.color = color;
    }

    #region Sound Effect
    private void playCrafting()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.MedicineCraft, Audio.Instance.SFXSource);
        }
    }
    #endregion

}
