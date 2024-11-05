using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class CampSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    //=====ITEM DATA=====//
    public ItemData itemData;
    public bool isFull;
    public RectTransform campSlot; // Reference to the camp slot RectTransform

    //temporary data for calculation
    public int datax;
    public int datay;
    public int dataIy; //positioning description

    //=====ITEM SLOT=====//
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject dropdownMenuPrefab;
    private GameObject activeDropdownMenu; //crearting dropdown
    private GameObject activeDropdownMenu_panel; //child
    public GameObject itemDescription;
    private GameObject itemDescription_panel;


    public event Action<CampSlot> OnItemClicked;

    // Reference to the inventory manager that handles the inventory open/close state
    [SerializeField] private InventoryController inventoryC;


    public GameObject P1;
    public GameObject P2;

    //detect if in range of the campsite or not
    [SerializeField] private ChestController ChestIn;

    private void Start()
    {
    }

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
        Vector3 popUpPosition = new Vector3(campSlot.position.x + datax, campSlot.position.y + datay);
        // Set the position of the pop-up menu
        activeDropdownMenu_panel.transform.position = popUpPosition;

        //find the button
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

        buttonS.onClick.AddListener(() => StoreItem());

    }

    private void StoreItem()
    {
        Debug.Log("Store item: " + itemData.itemName);
        // Logic to store it to item slot
        HideDropdownMenu();
    }
   

    private void RemoveItem()
    {
        itemData = null;
        isFull = false;
        itemImage.sprite = null;
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

    public void AddItemToSlot(ItemData newItem)
    {
        itemData = newItem;
        isFull = true;
        itemImage.sprite = itemData.itemSprite;
    }

    public void ClearSlot()
    {
        itemData = null;
        isFull = false;
        itemImage.sprite = null;
    }

    public bool IsEmpty()
    {
        return !isFull;
    }

}
