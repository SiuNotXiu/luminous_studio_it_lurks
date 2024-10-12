using UnityEngine;

public enum ItemType { Usable, Craftable, Upgradeable }

[CreateAssetMenu(fileName = "New Item", menuName = "Item for Inventory")]
public class Item : ScriptableObject
{
    public string itemName;
    public int ID => GetInstanceID();
    public Sprite icon;
    public ItemType itemType;
    public bool isCraftable;

    public virtual void UseItem()
    {
        // make a new script to override for each item use
        Debug.Log("Using item: " + itemName);
    }

    public virtual void DropItem()
    {

        Debug.Log("Dropping item: " + itemName);
    }

    public virtual void DiscardItem()
    {
        // removing the item from the inventory (future)
        Debug.Log("Discarding item: " + itemName);
    }

    public bool CanBeUsedAsCraftingMaterial()
    {
        return isCraftable; // allow item to be used in crafting even if it doesn't result in output
    }
}