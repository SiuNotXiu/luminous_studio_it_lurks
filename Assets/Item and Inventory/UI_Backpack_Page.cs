using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_Backpack_Page : MonoBehaviour
{
    [SerializeField] private UI_Inventory_Item itemPrefab;
    [SerializeField] private RectTransform contentPanel;

    private List<UI_Inventory_Item> listOfUIItems = new List<UI_Inventory_Item>();
    public void InitializeInventoryUI(int inventorysize)
    {
        for (int i = 0; i < inventorysize; i++)
        {
            UI_Inventory_Item uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);

            listOfUIItems.Add(uiItem);
            //the script was unperfect as it only keep collect without limiting, should limit to 6
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage)
    {
        if (listOfUIItems.Count > itemIndex)
        {
            listOfUIItems[itemIndex].SetData(itemImage);
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