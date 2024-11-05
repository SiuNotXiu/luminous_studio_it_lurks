using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string itemTag;
    public Sprite itemSprite;

    public bool isBulbCompatible;
    public bool isBatteryCompatible;
}
