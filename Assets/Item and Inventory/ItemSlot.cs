using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //=====ITEM DATA=====//
    public ItemData itemData;
    public bool isFull;
    public int slotIndex;
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
    private readonly float filledAlpha = 1f;     // item added (opaque)
    private readonly float emptyAlpha = 0f;    // item remove (transparent)
    private bool isSelected = false;

    private GameObject activeDropdownMenu; //crearting dropdown
    private GameObject activeDropdownMenu_panel; //child
    public GameObject itemDescription;
    private GameObject itemDescription_panel;


    public event Action<ItemSlot> OnItemClicked, OnRightMouseBtnClick;

    // Reference to the inventory manager that handles the inventory open/close state
    [SerializeField] private InventoryController inventoryC;
    public GameObject P1;
    public GameObject P2;

    //for items reference
    private PerkSlot perksEquip; //to check can use better battery or not
    [SerializeField] private HealthEffects playerHealth;
    [SerializeField] private battery_bar_float playerBattery;
    private bool isDropdownMenuActive = false;

    private void Update()
    {
        // Check if a click occurs outside the dropdown menu and item slot
        if (activeDropdownMenu != null && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                HideDropdownMenu(); // Hide the dropdown menu
                DeselectItem();
            }
        }
    }


    public void AddItem(ItemData itemData)
    {
        this.itemData = itemData;
        isFull = true;
        this.itemImage.sprite = itemData.itemSprite;

        SetItemImageAlpha(filledAlpha);
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
            if (isSelected)
            {
                DeselectItem();
            }
            else
            {
                SelectItem();
            }

            OnItemClicked?.Invoke(this);
            Debug.Log("Left click on item: " + itemData.itemName);
            ShowDropdownMenu();
        }
    }
    #region show the item name
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
    #endregion
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
        activeDropdownMenu_panel.transform.position = popUpPosition; // Set the position of the pop-up menu

        //find the button
        Button buttonS = activeDropdownMenu.transform.Find("Panel/Store").GetComponent<Button>();
        Button buttonC = activeDropdownMenu.transform.Find("Panel/Craft").GetComponent<Button>();
        Button buttonF = activeDropdownMenu.transform.Find("Panel/Fuse").GetComponent<Button>();
        Button buttonU = activeDropdownMenu.transform.Find("Panel/Use").GetComponent<Button>();
        Button buttonD = activeDropdownMenu.transform.Find("Panel/Drop").GetComponent<Button>();

        inventoryC.SetDropdownMenuInstance(activeDropdownMenu);

        //Debug.Log("IDK CHEcking : " + InventoryController.chest_detect.isInRange);
        if (InventoryController.chest_detect.isInRange)
        {
            buttonD.gameObject.SetActive(false);
        }
        if (P1.activeSelf)
        {
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
                //crafting
                buttonF.gameObject.SetActive(false);
            }
            else if (this.itemData.itemTag == "PerksItem")
            {
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
                    btn.onClick.AddListener(() => StoreItem(this));
                    playClick();
                    break;
                case "Craft":
                    btn.onClick.AddListener(() => CraftItem());
                    playClick();
                    break;
                case "Fuse":
                    btn.onClick.AddListener(() => FuseItem());
                    playClick();
                    break;
                case "Use":
                    btn.onClick.AddListener(() => UseItem());
                    playClick();
                    break;
                case "Drop":
                    btn.onClick.AddListener(() => DropItem());
                    playClick();
                    break;
            }
        }

        isDropdownMenuActive = true;

        SelectItem();
    }

    private void StoreItem(ItemSlot itemSlot)
    {
        Debug.Log("Store item: " + itemSlot.itemData.itemName);
        if (InventoryController.chest_detect.isInRange && InventoryController.chestOut.CanAddToChestInventory(itemSlot.itemData))
        {
            // Add the item to the chest
            Debug.Log("Item data>>>>>>>:" + itemSlot.itemData);
            InventoryController.chestOut.StoreItemFromPlayer(itemSlot.itemData, itemSlot.slotIndex);
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

        SetItemImageAlpha(emptyAlpha);
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
                battery_bar_float.reload_battery(battery_bar_float.which_battery_used.battery_normal);
                RemoveItem();
                break;

            case "1300 mAh Battery":
                //Batteries that have a battery life of 2.5 times longer than normal batteries. Requires an upgrade in order to use it
                if (battery_bar_float.reload_battery(battery_bar_float.which_battery_used.battery_1300_mah) == true)
                    RemoveItem();
                break;

            case "First Aid Kits":
                if (!playerHealth.GetFullHealth())
                {
                    playerHealth.FullHeal();
                    RemoveItem();
                }
                else if (playerHealth.GetFullHealth())
                {
                    Debug.Log("Player current health is max");
                }
                break;

            case "Bandage":
                if (!playerHealth.GetFullHealth())
                {
                    playerHealth.Heal();
                    RemoveItem();
                }
                else if (playerHealth.GetFullHealth())
                {
                    Debug.Log("Player current health is max");
                }
                break;

            case "Bushcraft Medicine":
                if (!playerHealth.GetFullHealth())
                {
                    playerHealth.Heal();
                    RemoveItem();
                }
                else if (playerHealth.GetFullHealth())
                {
                    Debug.Log("Player current health is max");
                }
                break;

            case "Adrenaline":
                //A syringe that makes the character move 1.5 times faster for 5 seconds
                TopdownMovement playerMovement = playerTransform.GetComponent<TopdownMovement>();
                if (playerMovement != null)
                {
                    playerMovement.UseAdrenaline();
                    RemoveItem();
                }
                break;

            default:
                Debug.Log("Unknown usage...: " + itemData.itemName);
                break;
        }

        DeselectItem();
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

    public void ClearSlot()
    {
        itemData = null;
        isFull = false;
        itemImage.sprite = null;

        SetItemImageAlpha(emptyAlpha);
    }

    public int GetSlotIndex()
    {
        return slotIndex;
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

        if (!isSelected)
        {
            isSelected = true;
            itemImage.sprite = itemData.selectedSprite;
        }
    }

    public void DeselectItem()
    {
        if (itemData != null && isSelected)
        {
            isSelected = false;
            itemImage.sprite = itemData.itemSprite;
        }
    }

    public void ResetToDefaultSprite()
    {
        if (itemData != null)
        {
            isSelected = false;
            itemImage.sprite = itemData.itemSprite;
        }
    }

    #region Sound Effect
    private void playClick()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.UIHoverAndClick, Audio.Instance.SFXSource);
        }
    }

    #endregion
}
