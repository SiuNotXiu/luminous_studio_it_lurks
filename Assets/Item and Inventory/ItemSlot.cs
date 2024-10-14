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
    public GameObject popUpMenu; // Reference to the pop-up menu(activeDropdownMenu)
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
    private GameObject activeDropdownMenu_panel;

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

        // Instantiate the dropdown menu prefab
        activeDropdownMenu = Instantiate(dropdownMenuPrefab);
        activeDropdownMenu_panel = activeDropdownMenu.transform.Find("Panel").gameObject;

        // Get the RectTransform of the dropdown menu
        RectTransform dropdownRectTransform = activeDropdownMenu.GetComponent<RectTransform>();

        // Calculate the new position for the pop-up menu
        Vector3 popUpPosition = new Vector3(itemSlot.position.x + datax, itemSlot.position.y);

        // Set the position of the pop-up menu
        //dropdownRectTransform.position = popUpPosition;
        activeDropdownMenu_panel.transform.position = popUpPosition;
        //activeDropdownMenu_panel.transform.localPosition = new Vector3(datax, 0);
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
