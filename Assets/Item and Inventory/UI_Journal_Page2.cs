using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Journal_Page2 : MonoBehaviour
{
    [SerializeField]
    private UI_Inventory_Item itemPrefab; //prefab slot for item UI

    [SerializeField]
    private RectTransform contentPanenl; // panel for items

    private List<UI_Inventory_Item> listOfUIItems = new List<UI_Inventory_Item>();
    private InventorySystem inventory;

    private void Start()
    {
        // search for active InventorySystem in the scene
        inventory = FindObjectOfType<InventorySystem>();
        if (inventory != null)
        {
            InitializeInventoryUI(inventory);
        }
        else
        {
            Debug.LogError("InventorySystem not found!");
        }
    }

    public void InitializeInventoryUI(InventorySystem inv)
    {
        inventory = inv;

        int inventorySize = Mathf.Min(inventory.inventorySize, 6);

        for(int i = 0;i < inventorySize; i++)
        {
            UI_Inventory_Item uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanenl, false);
            listOfUIItems.Add(uiItem);
        }

        RefreshInventoryUI();
    }

    // update UI based on inventory data
    public void UpdateData(int itemIndex, Item item)
    {
        if (listOfUIItems.Count > itemIndex)
        {
            UI_Inventory_Item uiItem = listOfUIItems[itemIndex];
            uiItem.SetData(item.icon);
            uiItem.AssignInventorySlot(item, inventory);
        }
    }

    // add item on empty slot and call update
    public void AddNewItem(Item item)
    {
        if (inventory.AddItem(item))
        {
            Debug.Log("Item has added");
            RefreshInventoryUI();
        }
    }

    // keep update to latest state
    public void RefreshInventoryUI()
    {
        foreach (UI_Inventory_Item uiItem in listOfUIItems)
        {
            uiItem.ClearData();
        }

        for (int i = 0; i < inventory.items.Count; i++)
        {
            UpdateData(i, inventory.items[i]);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
