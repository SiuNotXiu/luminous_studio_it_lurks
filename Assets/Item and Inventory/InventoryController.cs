using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UI_Journal_Page2 JinventoryUI;
    [SerializeField] private Journal_display journal_Display;
    [SerializeField] private Button_display button_display;
    [SerializeField] private UI_Backpack_Page BackpackUI;

    [SerializeField] private UI_Chest_Page ChestUI;
    [SerializeField] private ChestController DetectChest;
    public UnityEvent interactAction;

    // chermin inventory
    [SerializeField] private InventorySO inventoryData;

    private void Start()
    {
        JinventoryUI.InitializeInventoryUI(inventoryData.Size);
        inventoryData.Initialize();
    }

    public void Update()
    {
  
        if (Input.GetKeyDown(KeyCode.Tab))//Journal
        {
            Debug.Log("tab1");
            Debug.Log("journal_Display.isActiveAndEnabled > "+ journal_Display.isActiveAndEnabled);
            if (journal_Display.isActiveAndEnabled == false) 
            {

                button_display.Show();
                button_display.ShowPanels();

                journal_Display.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    BackpackUI.UpdateData(item.Key, item.Value.item.ItemImage);
                }
                journal_Display.ShowPanels();

                BackpackUI.Hide();
                ChestUI.Hide();
            }
            else
            {
                journal_Display.HidePanels();
                journal_Display.Hide();

                button_display.HidePanels();
                button_display.Hide();
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))//Backpack
        {
            if (BackpackUI.isActiveAndEnabled == false)
            {
                BackpackUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    BackpackUI.UpdateData(item.Key, item.Value.item.ItemImage);
                }
                ChestUI.Hide();
                journal_Display.HidePanels();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    JinventoryUI.UpdateData(item.Key, item.Value.item.ItemImage);
                }
                button_display.HidePanels();

                journal_Display.Hide();
                button_display.Hide();
            }
            else
            {
                BackpackUI.Hide();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E)) // Chest
        {
            if (DetectChest.isInRange) 
            {
                if (ChestUI.isActiveAndEnabled == false) 
                {
                    interactAction.Invoke(); // Interact with chest
                    ChestUI.Show();
                    journal_Display.HidePanels();
                    button_display.HidePanels();
                    BackpackUI.Hide();

                    journal_Display.Hide();
                    button_display.Hide();
                    //PlayerMovement.enabled = false;
                }
                else
                {
                    ChestUI.Hide();
                    //PlayerMovement.enabled = true; 
                }
            }
            else 
            {
                ChestUI.Hide();
                //PlayerMovement.enabled = true; // Ensure player can move if UI is hidden
            }
        }
    }
}
