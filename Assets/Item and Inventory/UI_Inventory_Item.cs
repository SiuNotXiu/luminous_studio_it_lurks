using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory_Item : MonoBehaviour
{ 
    
    [SerializeField] private Image itemImage;

   public event Action<UI_Inventory_Item> OnItemClicked, OnRightMouseBtnClick;

   private bool empty = true;

    public void Awake()
    {
        ResetData();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        this.empty = true;
    }


    public void SetData(Sprite sprite)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.empty = false;
    }

    public void OnPointerClick(BaseEventData data)
    {
        /*
        if (this.empty)
        {
            return;
        }
        */
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
            Debug.Log("Right click on item: " + this.name);
        }
        else
        {
            OnItemClicked?.Invoke(this);
            Debug.Log("Left click on item: " + this.name);
        }
    }
}
