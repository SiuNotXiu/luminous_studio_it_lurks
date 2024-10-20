using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //=====ITEM DATA=====//
    public string itemName;
    public Sprite itemSprite;
    public string itemTag;
    public bool isFull;
    public RectTransform itemSlot; // Reference to the item slot RectTransform

    //temporary data for calculation
    public int datax;
    public int datay;

    //=====ITEM SLOT=====//
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject dropdownMenuPrefab;
    private GameObject activeDropdownMenu;
    private GameObject activeDropdownMenu_panel; //child
    

    public event Action<ItemSlot> OnItemClicked, OnRightMouseBtnClick;

    // Reference to the inventory manager that handles the inventory open/close state
    [SerializeField] private InventoryController inventoryC;
    public GameObject P1;
    public GameObject P2;


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

    public void AddItem(string itemName, string itemTag, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        Debug.Log(itemTag + "is the rechange tag");
        this.itemTag = itemTag;
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
        else if (eventData.button == PointerEventData.InputButton.Right)//we may only using left, still need confirm
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

        //find the button
        Button buttonS = activeDropdownMenu.transform.Find("Panel/Store").GetComponent<Button>();
        Button buttonC = activeDropdownMenu.transform.Find("Panel/Craft").GetComponent<Button>();
        Button buttonF = activeDropdownMenu.transform.Find("Panel/Fuse").GetComponent<Button>();
        Button buttonU = activeDropdownMenu.transform.Find("Panel/Use").GetComponent<Button>();
        Button buttonD = activeDropdownMenu.transform.Find("Panel/Drops").GetComponent<Button>();

        inventoryC.SetDropdownMenuInstance(activeDropdownMenu);

        //storing
        if (P1.activeSelf)
        {
            Debug.Log("Detect it is on p1: " + P1.activeSelf);
            //storing

            buttonC.gameObject.SetActive(false);
            buttonF.gameObject.SetActive(false);
        }
        else if (!P1.activeSelf)
        {
            if (this.itemTag == "CraftItem")
            {
                Debug.Log("Detect it is on p2: " + P2.activeSelf);
                //crafting

                buttonS.gameObject.SetActive(false);
                buttonF.gameObject.SetActive(false);
            }
            else if (this.itemTag == "PerksItem")
            {
                Debug.Log("Detect it is on p2: " + P2.activeSelf);
                //fuse
                buttonS.gameObject.SetActive(false);
                buttonU.gameObject.SetActive(false);
                buttonC.gameObject.SetActive(false);
            }

        }

        Button[] buttons = activeDropdownMenu.GetComponentsInChildren<Button>();
        foreach (Button btn in buttons)
        {
            switch (btn.name)
            {
                case "Store":
                    Debug.Log("Store button work");
                    btn.onClick.AddListener(() => StoreItem());
                    break;
                case "Craft":
                    btn.onClick.AddListener(() => CraftItem());
                    break;
                case "Fuse":
                    btn.onClick.AddListener(() => FuseItem());
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

    private void StoreItem()
    {
        Debug.Log("Store item: " + itemName);
        // Logic to discard the item
        HideDropdownMenu();
    }
    private void CraftItem()
    {
        Debug.Log("Crafting item: " + itemName);
        CraftingSlot craftingSlot = FindObjectOfType<CraftingSlot>();

        if (craftingSlot != null)
        {
            bool addedToCrafting = craftingSlot.AddItemToCraftingSlot(this);

            if (addedToCrafting)
            {
                //success
                RemoveItem();
            }
            else
            {
                Debug.Log("No available crafting slots.");
            }
        }
        HideDropdownMenu();
    }

    private void RemoveItem()
    {
        itemName = "";
        itemSprite = null;
        itemTag = "";
        isFull = false;
        itemImage.sprite = null;
    }

    private void FuseItem()
    {
        Debug.Log("Store item: " + itemName);
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
