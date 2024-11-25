using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public Vector3 defaultScale = Vector3.one;
    public string itemName;
    public string itemTag;
    public Sprite itemSprite;
    public Sprite droppedSprite;
    public Sprite selectedSprite;

    public bool isBulbCompatible;
    public bool isBatteryCompatible;

    [Header("For keys lol dashaB")]
    public bool isKey;
    public string gateID;
}
