using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_Item : MonoBehaviour
{
    [SerializeField] private Image itemIcon;  
    [SerializeField] private Text itemName;   


    public void SetItem(Item item)
    {
        itemIcon.sprite = item.itemIcon; 
        itemName.text = item.itemName;   
        gameObject.SetActive(true);       
    }

    public void ClearSlot()
    {
        itemIcon.sprite = null;          
        gameObject.SetActive(false);     
    }
}
