using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemIcon; 
    private Item currentItem;
    private InventorySystem inventory;

    public Dropdown itemDropdown; 

    public void SetupSlot(Item newItem, InventorySystem inv)
    {
        currentItem = newItem;
        inventory = inv;
        itemIcon.sprite = newItem.icon;

        /*
        itemDropdown.options.Clear();
        itemDropdown.options.Add(new Dropdown.OptionData("Use"));
        itemDropdown.options.Add(new Dropdown.OptionData("Drop"));
        itemDropdown.options.Add(new Dropdown.OptionData("Discard"));

        itemDropdown.onValueChanged.AddListener(OnDropdownSelect);
        */
    }

    public void OnDropdownSelect(int index)
    {
        switch (index)
        {
            case 0:
                inventory.UseItem(currentItem);
                break;
            case 1:
                inventory.DropItem(currentItem);
                break;
            case 2:
                inventory.DiscardItem(currentItem);
                break;
        }

        // Reset dropdown after selection
        itemDropdown.value = 0;
    }
}