using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //=====ITEM DATA=====//
    public string itemName;
    public Sprite itemSprite;
    public bool isFull;

    //=====ITEM SLOT=====//
    [SerializeField] private Image itemImage;

    // Dropdown menu prefab reference
    [SerializeField] private GameObject dropdownMenuPrefab;

    // Keep track of the dropdown menu instance
    private GameObject activeDropdownMenu;

    public event Action<ItemSlot> OnItemClicked, OnRightMouseBtnClick;

    public void AddItem(string itemName, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        isFull = true;
        this.itemImage.sprite = itemSprite;
    }


    public bool IsEmpty()
    {
        return !isFull;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isFull)
        {
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnItemClicked?.Invoke(this);
            Debug.Log("Left click on item: " + itemName);
            ShowDropdownMenu();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
            Debug.Log("Right click on item: " + itemName);

            // Show dropdown menu on right-click
            ShowDropdownMenu();
        }
    }

    private void ShowDropdownMenu()
    {
        Debug.Log("Dropdown Trigger");
        if (activeDropdownMenu != null)
        {
            Destroy(activeDropdownMenu);
        }

        // Instantiate the dropdown menu prefab at the slot's position
        activeDropdownMenu = Instantiate(dropdownMenuPrefab, transform.position, Quaternion.identity, transform.parent);

        // Adjust the position to be below the item slot or in a visible area
        RectTransform rt = activeDropdownMenu.GetComponent<RectTransform>();

        // Set the dropdown's z position to 2
        Vector3 newPosition = transform.position + new Vector3(0, -rt.rect.height / 2, 0);
        newPosition.z = 2; // Adjust z position to be 2
        rt.position = newPosition;

        // Add listeners to the dropdown buttons
        Button[] buttons = activeDropdownMenu.GetComponentsInChildren<Button>();
        foreach (Button btn in buttons)
        {
            switch (btn.name)
            {
                case "ManyFunctionable":
                    Debug.Log("Work Many Function");
                    btn.onClick.AddListener(() => FunctionableItem());
                    break;
                case "Use":
                    btn.onClick.AddListener(() => UseItem());
                    break;
                case "Drops":
                    btn.onClick.AddListener(() => DropItem());
                    break;
            }
        }
    }
    private void FunctionableItem()
    {
        Debug.Log("Functionable item: " + itemName);
        // Logic to discard the item
        HideDropdownMenu();
    }
    private void UseItem()
    {
        Debug.Log("Using item: " + itemName);
        // Logic to use the item
        HideDropdownMenu();
    }

    private void DropItem()
    {
        Debug.Log("Dropping item: " + itemName);
        // Logic to drop the item
        HideDropdownMenu();
    }


    private void HideDropdownMenu()
    {
        if (activeDropdownMenu != null)
        {
            Destroy(activeDropdownMenu);
            activeDropdownMenu = null;
        }
    }
}
