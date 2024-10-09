using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory_Item : MonoBehaviour
{
    /*
    [SerializeField] private Image itemImage;  
    [SerializeField] private Text itemName;

    [SerializeField] private Image borderImage;

    public event Action<UI_Inventory_Item> OnItemClicked;

    private bool empty = true;
    public void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
       this.itemImage.gameObject.SetActive(false);
       empty = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false;
    }

    public void SetData(Sprite sprite)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        empty = false;
    }

    public void Select()
    {
        borderImage.enabled = true;
    }

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Left)
        {
            OnItemClicked?.Invoke(this);
        }
        else
        {
            //
        }
    }
    */
}
