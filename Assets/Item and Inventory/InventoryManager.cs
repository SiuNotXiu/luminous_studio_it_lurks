using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;

    [SerializeField] private InventoryController inventoryController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && menuActivated)
        {
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !menuActivated)
        {
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }
    public void AddItem(string itemName, Sprite itemSprite)
    {
        if (!inventoryController.ArePanelsOpen())
        {
            Debug.Log("Adding item: " + itemName);
            // Add item to inventory logic here
        }
        else
        {
            Debug.Log("Cannot add item: Panels are open");
        }
    }
    
}
