using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class ItemSlot : MonoBehaviour
{
    //=====ITEM DATA=====//
    public string itemName;
    public Sprite itemSprite;
    public bool isFull;

    //=====ITEM SLOT=====//
    [SerializeField] private Image itemImage;

    public void AddItem(string itemName, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        isFull = true;

        itemImage.sprite = itemSprite;
    }

}
