using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //=====ITEM DATA=====//
    public ItemData itemData;
    public bool isFull;
    public RectTransform itemSlot; // Reference to the item slot RectTransform
    [SerializeField] private GameObject droppedItemPrefab; // reference to the item prefab (for dropping function)
    [SerializeField] private Transform playerTransform;

    //temporary data for calculation
    public int datax;
    public int datay;
    public int dataIx;//this is for the item description 
    public int dataIy;

    //=====ITEM SLOT=====//
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject dropdownMenuPrefab;
    private GameObject activeDropdownMenu; //crearting dropdown
    private GameObject activeDropdownMenu_panel; //child
    public GameObject itemDescription;
    private GameObject itemDescription_panel;


    public event Action<ItemSlot> OnItemClicked, OnRightMouseBtnClick;

    // Reference to the inventory manager that handles the inventory open/close state
    [SerializeField] private InventoryController inventoryC;
    public GameObject P1;
    public GameObject P2;

    //detect if in range of the campsite or not
    [SerializeField] private ChestController ChestIn;
    [SerializeField] private ChestInventory chestInventory;

    

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

    public void AddItem(ItemData itemData)
    {
        this.itemData = itemData;
        isFull = true;
        this.itemImage.sprite = itemData.itemSprite;
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
            Debug.Log("Left click on item: " + itemData.itemName);
            ShowDropdownMenu();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)//we may only using left, still need confirm
        {
            OnRightMouseBtnClick?.Invoke(this);
            Debug.Log("Right click on item: " + itemData.itemName);
            ShowDropdownMenu();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //not need to create one just make it position to the item
        if (!isFull)
        {
            return;
        }
        else
        {
            //positioning the item pop up description
            itemDescription.SetActive(true);
            itemDescription_panel = itemDescription.transform.Find("Panel").gameObject;
            Vector3 itemdescriptionPosition =  new Vector3(itemSlot.position.x + dataIx, itemSlot.position.y + dataIy);
            itemDescription_panel.transform.position = itemdescriptionPosition;

            //Find the button text and change the text to the item data
            Button buttonI = itemDescription.transform.Find("Panel/description").GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonI.GetComponentInChildren<TextMeshProUGUI>(); // Get the Text component of the button

            if (buttonText != null)
            {
                Debug.Log("Name has change");
                buttonText.text = itemData.itemName; // Set the button text to the item's name or any data
            }


        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDescription.SetActive(false);
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
        Button buttonD = activeDropdownMenu.transform.Find("Panel/Drop").GetComponent<Button>();

        inventoryC.SetDropdownMenuInstance(activeDropdownMenu);

        if (ChestIn.isInRange)
        {
            buttonD.gameObject.SetActive(false);
        }
        if (P1.activeSelf)
        {
            Debug.Log("Detect it is on p1: " + P1.activeSelf);
            //storing
            if (this.itemData.itemTag == "PerksItem")
            {
                buttonU.gameObject.SetActive(false);
            }
            buttonC.gameObject.SetActive(false);
            buttonF.gameObject.SetActive(false);
        }
        else if (!P1.activeSelf)
        {
            buttonS.gameObject.SetActive(false);
            if (this.itemData.itemTag == "CraftItem")
            {
                Debug.Log("Detect it is on p2: " + P2.activeSelf);
                //crafting
                buttonF.gameObject.SetActive(false);
            }
            else if (this.itemData.itemTag == "PerksItem")
            {
                Debug.Log("Detect it is on p2: " + P2.activeSelf);
                //fuse
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
                case "Drop":
                    btn.onClick.AddListener(() => DropItem());
                    break;
            }
        }
    }

    private void StoreItem()
    {
        Debug.Log("Store item: " + itemData.itemName);
        if (ChestIn.isInRange && chestInventory.CanAddToChestInventory(itemData))
        {
            // Add the item to the chest
            chestInventory.StoreItemFromPlayer(itemData);
            ClearSlot();
        }
        else
        {
            Debug.Log("Cannot store item: Chest is either full or out of range.");
        }
        HideDropdownMenu();
    }
    private void CraftItem()
    {
        Debug.Log("Crafting item: " + itemData.itemName);

        //check for craftingslot to add
        if (inventoryC.AddItemToCraftingSlot(itemData))
        {
            RemoveItem();

            // attempt crafting once items are added
            inventoryC.TryCrafting();
        }
        else
        {
            Debug.Log("No available crafting slots.");
        }
        HideDropdownMenu();
    }

    private void RemoveItem()
    {
        itemData = null;
        isFull = false;
        itemImage.sprite = null;
    }

    private void FuseItem()
    {
        Debug.Log("Store item: " + itemData.itemName);
        if (itemData.isBulbCompatible || itemData.isBatteryCompatible)
        {
            inventoryC.FuseItemToPerkSlot(itemData);
            RemoveItem();
        }
        else
        {
            Debug.Log("Item cannot be used in perk slots.");
        }
        HideDropdownMenu();
    }

    private void UseItem()
    {
        if (itemData == null) return;
        Debug.Log("Using item: " + itemData.itemName);

        switch (itemData.itemName)
        {
            case "Battery":
                //Refills flashlight
                break;

            case "1300 mAh Battery":
                //Batteries that have a battery life of 2.5 times longer than normal batteries. Requires an upgrade in order to use it
                break;
            case "First Aid Kit":
                //Restores full health
                break;

            case "Bandage":
                //Restores 1 hit
                break;

            case "Adrenaline":
                //A syringe that makes the character move 1.5 times faster for 5 seconds
                break;

            default:
                Debug.Log("Unknown usage: " + itemData.itemName);
                break;
        }
        HideDropdownMenu();
    }

    private void DropItem()
    {
        if (itemData == null) return;

        Debug.Log("Dropping item: " + itemData.itemName);

        Vector3 dropPosition = playerTransform.position + new Vector3(0, 0.5f, 0);
        GameObject droppedItem = Instantiate(droppedItemPrefab, dropPosition, Quaternion.identity);

        DroppedItem droppedItemScript = droppedItem.GetComponent<DroppedItem>();
        if (droppedItemScript != null)
        {
            droppedItemScript.Initialize(itemData);
        }

        ClearSlot();
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

    public void ClearSlot()
    {
        itemData = null;
        isFull = false;
        itemImage.sprite = null;
    }
}
