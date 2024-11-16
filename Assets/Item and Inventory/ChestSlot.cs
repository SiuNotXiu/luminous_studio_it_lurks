using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class ChestSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    //=====ITEM DATA=====//
    public ItemData itemData;
    public bool isFull;
    public RectTransform campSlot; // Reference to the camp slot RectTransform
    private GameObject clickedObject;

    //temporary data for calculation
    private int datax = -110;
    private int datay = -70;
    public int dataIy; //positioning description

    //=====ITEM SLOT=====//
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject dropdownMenuPrefab;
    private readonly float filledAlpha = 1f;     // item added (opaque)
    private readonly float emptyAlpha = 0f;    // item remove (transparent)
    private bool isSelected = false;
    private GameObject activeDropdownMenu; //crearting dropdown
    private GameObject activeDropdownMenu_panel; //child
    public GameObject itemDescription;
    private GameObject itemDescription_panel;


    public event Action<ChestSlot> OnItemClicked;

    // Reference to the inventory manager that handles the inventory open/close state
    [SerializeField] private InventoryController inventoryC;


    public GameObject P1;
    public GameObject P2;

    private bool isDropdownMenuActive = false;

    //should have a detect tell player the campsite is full and and store anymore
    private void Update()
    {

        // Check if a click occurs outside the dropdown menu and item slot
        if (activeDropdownMenu != null && Input.GetMouseButtonDown(0))
        {
            // Check if the pointer is over any UI element
            if (!IsPointerOverUIObject())
            {
                HideDropdownMenu(); // Hide the menu if clicked outside
                DeselectItem();
            }
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isFull)
        {
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left) //only using left
        {
            if (isSelected)
            {
                DeselectItem();
            }
            else
            {
                SelectItem();
            }
            clickedObject = null;
            clickedObject = gameObject;
            OnItemClicked?.Invoke(this);
            Debug.Log("Left click on item: " + itemData.itemName);
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
            Vector3 itemdescriptionPosition = new Vector3(campSlot.position.x, campSlot.position.y + dataIy);
            itemDescription_panel.transform.position = itemdescriptionPosition;

            //Find the button text and change the text to the item data
            Button buttonI = itemDescription.transform.Find("Panel/description").GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonI.GetComponentInChildren<TextMeshProUGUI>(); // Get the Text component of the button

            if (buttonText != null)
            {
                buttonText.text = itemData.itemName; // Set the button text to the item's name or any data
            }


        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDescription.SetActive(false);
    }
    public ItemData GetItemData()
    {
        return itemData;
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

        // Calculate the new position for the pop-up menu
        Vector3 popUpPosition = new Vector3(campSlot.position.x + datax, campSlot.position.y + datay);
        // Set the position of the pop-up menu
        activeDropdownMenu_panel.transform.position = popUpPosition;

        // Find the buttons
        Button buttonS = activeDropdownMenu.transform.Find("Panel/Store").GetComponent<Button>();
        Button buttonC = activeDropdownMenu.transform.Find("Panel/Craft").GetComponent<Button>();
        Button buttonF = activeDropdownMenu.transform.Find("Panel/Fuse").GetComponent<Button>();
        Button buttonU = activeDropdownMenu.transform.Find("Panel/Use").GetComponent<Button>();
        Button buttonD = activeDropdownMenu.transform.Find("Panel/Drop").GetComponent<Button>();

        inventoryC.SetDropdownMenuInstance(activeDropdownMenu);

        buttonS.gameObject.SetActive(true);
        buttonC.gameObject.SetActive(false);
        buttonF.gameObject.SetActive(false);
        buttonU.gameObject.SetActive(false);
        buttonD.gameObject.SetActive(false);

        // Corrected listener
        buttonS.onClick.AddListener(() => StoreItem(itemData)); // Use itemData instead of itemdata

        isDropdownMenuActive = true;

        SelectItem();
    }


    private void StoreItem(ItemData item)
    {
        Debug.Log("Store item: " + item.itemName);
        HideDropdownMenu();

        if (inventoryC.CanAddToPlayerInventory(item))
        {
            inventoryC.AddItemToPlayerInventory(item);
            ClearSlot();

            Debug.Log("Item stored in player inventory: " + item.itemName);
        }
        else
        {
            Debug.Log("Player inventory is full, cannot store item.");
        }                                                                   
    }


    private void RemoveItem()
    {
        itemData = null;
        isFull = false;
        itemImage.sprite = null;

        SetItemImageAlpha(emptyAlpha);
    }

    public void HideDropdownMenu()
    {
        if (activeDropdownMenu != null)
        {
            Debug.Log("Destroy");
            Destroy(activeDropdownMenu);
            activeDropdownMenu = null;
        }

        DeselectItem();

        isDropdownMenuActive = false;
    }

    private bool IsPointerOverUIObject()
    {
        // Create a new PointerEventData to store the current pointer position
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // Perform a raycast to check for UI elements
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            // Check if the pointer is over the dropdown menu, item slot, or their children
            if (result.gameObject == gameObject ||
                result.gameObject == activeDropdownMenu ||
                result.gameObject.transform.IsChildOf(activeDropdownMenu.transform))
            {
                return true; // Pointer is over the relevant UI
            }
        }

        return false; // Pointer is not over relevant UI
    }

    public void AddItem(ItemData newItem)
    {
        if (newItem == null)
        {
            Debug.LogError("AddItemToSlot: newItem is null.");
            return;
        }

        if (itemImage == null)
        {
            Debug.LogError("AddItemToSlot: itemImage is not assigned.");
            return;
        }

        this.itemData = newItem;
        isFull = true;
        this.itemImage.sprite = newItem.itemSprite;

        SetItemImageAlpha(filledAlpha);
    }


    public void ClearSlot()
    {
        this.itemData = null;
        isFull = false;
        this.itemImage.sprite = null;

        SetItemImageAlpha(emptyAlpha);
    }

    public bool IsEmpty()
    {
        return !isFull;
    }


    private void SetItemImageAlpha(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    private void SelectItem()
    {
        if (!isDropdownMenuActive) return;
        isSelected = true;
        itemImage.sprite = itemData.selectedSprite;
    }

    public void DeselectItem()
    {
        isSelected = false;
        itemImage.sprite = itemData.itemSprite;
    }
}
