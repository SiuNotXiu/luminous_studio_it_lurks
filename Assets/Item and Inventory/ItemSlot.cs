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
    public RectTransform itemSlot; // Reference to the item slot RectTransform

    //temporary data for calculation
    public int datax;
    public int datay;

    //=====ITEM SLOT=====//
    [SerializeField] private Image itemImage;

    // Dropdown menu prefab reference
    [SerializeField] public GameObject dropdownMenuPrefab;

    // Keep track of the dropdown menu instance
    private GameObject activeDropdownMenu;
    private GameObject activeDropdownMenu_panel; //child

    public event Action<ItemSlot> OnItemClicked, OnRightMouseBtnClick;

    // Reference to the inventory manager that handles the inventory open/close state
    [SerializeField] private InventoryController inventoryManager;

    private void Update()
    {
        // Check if a click occurs outside the dropdown menu and item slot
        if (activeDropdownMenu != null && Input.GetMouseButtonDown(0))
        {
            // Check if the pointer is over any UI element
            if (!IsPointerOverUIObject())
            {
                HideDropdownMenu(); // Hide the menu if clicked outside
            }
        }

    }

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

        // Instantiate the dropdown menu prefab
        activeDropdownMenu = Instantiate(dropdownMenuPrefab);
        activeDropdownMenu_panel = activeDropdownMenu.transform.Find("Panel").gameObject; //from child find it from parent

        // Calculate the new position for the pop-up menu
        Vector3 popUpPosition = new Vector3(itemSlot.position.x + datax, itemSlot.position.y + datay);

        // Set the position of the pop-up menu
        activeDropdownMenu_panel.transform.position = popUpPosition;

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

    public void HideDropdownMenu()
    {
        if (activeDropdownMenu != null)
        {
            Debug.Log("Destroy");
            Destroy(activeDropdownMenu);
            activeDropdownMenu = null;
        }
    }

    private bool IsPointerOverUIObject()
    {
        // Check if the pointer is over a UI element, either the item slot or the dropdown menu
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition;
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            // If the raycast hit the item slot or the dropdown menu, return true
            if (result.gameObject == gameObject || result.gameObject == activeDropdownMenu)
            {
                return true;
            }
        }
        return false;
    }
}
