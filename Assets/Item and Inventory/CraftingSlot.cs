using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot : MonoBehaviour
{
    [SerializeField] private CraftingSlot[] craftingSlots = new CraftingSlot[2];
    [SerializeField] private ResultSlot resultSlot;
    private Dictionary<string, string> knownRecipes;
    private List<ItemSlot> tempItems = new List<ItemSlot>();
    private InventoryController inventoryController;

    //=====ITEM DATA=====//
    public string itemName;
    public Sprite itemSprite;
    public string itemTag;
    public bool isFull;

    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GameObject.Find("Journal_Canvas").GetComponent<InventoryController>();
        InitializeRecipes();
    }

    // add recipes here in future
    private void InitializeRecipes()
    {
        knownRecipes = new Dictionary<string, string>
        {
            // example recipe: combine "ItemA" and "ItemB" to get "ResultItem"
            { "ItemA+ItemB", "ResultItem" }
        };
    }

    public void AddItem(string itemName, string itemTag, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.itemTag = itemTag;
        this.isFull = true;
        Debug.Log("Item added to CraftingSlot: " + itemName);
    }

    public bool IsEmpty()
    {
        return !isFull;
    }

    public ItemSlot RemoveItem()
    {
        ItemSlot temp = new ItemSlot
        {
            itemName = this.itemName,
            itemTag = this.itemTag,
            itemSprite = this.itemSprite,
            isFull = this.isFull
        };

        this.itemName = "";
        this.itemSprite = null;
        this.itemTag = "";
        this.isFull = false;

        return temp;
    }
    public void ClearSlot()
    {
        this.itemName = "";
        this.itemSprite = null;
        this.itemTag = "";
        this.isFull = false;
    }

    public bool HasItem()
    {
        return isFull;
    }

    public string GetItemName()
    {
        return itemName;
    }



    public bool AddItemToCraftingSlot(ItemSlot itemSlot)
    {
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            if (craftingSlots[i].IsEmpty())
            {
                craftingSlots[i].AddItem(itemSlot.itemName, itemSlot.itemTag, itemSlot.itemSprite);
                tempItems.Add(itemSlot);
                CheckAndCraft();
                return true;
            }
        }

        return false;
    }

    private void CheckAndCraft()
    {
        if (craftingSlots[0].isFull && craftingSlots[1].isFull)
        {
            string item1Name = craftingSlots[0].GetItemName();
            string item2Name = craftingSlots[1].GetItemName();

            string recipeKey = item1Name + "+" + item2Name;

            if (knownRecipes.ContainsKey(recipeKey))
            {
                string resultItemName = knownRecipes[recipeKey];
                Sprite resultSprite = GetItemSprite(resultItemName); // Fetch the sprite for the result item

                // removed items remembered
                tempItems.Add(craftingSlots[0].RemoveItem());
                tempItems.Add(craftingSlots[1].RemoveItem());

                resultSlot.ShowResult(resultItemName, resultSprite);
            }
        }
    }

    public void OnClickAddToInventory()
    {
        if (HasItem() && !inventoryController.IsInventoryFull())
        {
            string resultItemName = GetItemName();
            Sprite resultItemSprite = GetItemSprite(resultItemName);

            inventoryController.AddItem(resultItemName, "Crafted", resultItemSprite);

            ClearSlot();
        }
    }

    public void CancelCrafting()
    {
        foreach (ItemSlot itemSlot in tempItems)
        {
            inventoryController.AddItem(itemSlot.itemName, itemSlot.itemTag, itemSlot.itemSprite);
        }
        tempItems.Clear();
    }

    // Helper method to get the sprite based on item name (to be implemented)
    private Sprite GetItemSprite(string itemName)
    {
        // Your logic to fetch the sprite from a resource manager or database
        return null;
    }
}
