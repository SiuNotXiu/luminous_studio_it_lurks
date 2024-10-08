using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Backpack_Page : MonoBehaviour
{
    [SerializeField] private UI_Inventory_Item itemPrefab;
    [SerializeField] private RectTransform contentPanel;

    private List<UI_Inventory_Item> listOfUIItems = new List<UI_Inventory_Item>();
    public void InitializeInventoryUI(int inventorysize)
    {
        for (int i = 0; i < inventorysize; i++)
        {
            UI_Inventory_Item uiItem = Instantiate(itemPrefab,contentPanel);
            listOfUIItems.Add(uiItem);
            //the script was unperfect as it only keep collect without limiting, should limit to 6
        }
    }

    public void UpdateUI(List<Item> inventoryItems)
    {
        for (int i = 0; i < listOfUIItems.Count; i++)
        {
            if (i < inventoryItems.Count)
            {
                // got item display in UI
                listOfUIItems[i].SetItem(inventoryItems[i]);  // need help set item in UI slot
            }
            else
            {
                // If no item, clear the UI slot
                listOfUIItems[i].ClearSlot();
            }
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
