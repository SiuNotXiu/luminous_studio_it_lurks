using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BackpackInventory : MonoBehaviour
{
    public List<Item> backpackInventorySpace = new List<Item>();  
    public int backpackInventorySize = 6;
    [SerializeField] private UI_Backpack_Page inventoryUIPage;

    public void AddItem(Item newItem)
    {
        if (backpackInventorySpace.Count < backpackInventorySize)
        {
            backpackInventorySpace.Add(newItem);
            Debug.Log(newItem.itemName + " added into inventory");
            UpdateInventoryUI();  
        }
    }

    private void UpdateInventoryUI()
    {
       // inventoryUIPage.UpdateUI(backpackInventorySpace);
        // this part u can integrate my script into urs UI or ur UI into here also can meowwww
    }
}