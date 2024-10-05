using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviourScript : MonoBehaviour
{
    [SerializeField] private UI_Journal_Page JinventoryUI;
    [SerializeField] private UI_Backpack_Page BackpackUI;
    [SerializeField] private UI_Chest_Page ChestUI;
    [SerializeField] private ChestController DetectChest;
    public UnityEvent interactAction;

    private int inventorySize = 6;
    private void Start()
    {
        JinventoryUI.InitializeInventoryUI(inventorySize);

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))//Journal
        {
            if (JinventoryUI.isActiveAndEnabled == false) 
            {
                JinventoryUI.Show();
                BackpackUI.Hide();
                ChestUI.Hide();
            }
            else
            {
                JinventoryUI.Hide();
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))//Backpack
        {
            if (BackpackUI.isActiveAndEnabled == false)
            {
                BackpackUI.Show();
                JinventoryUI.Hide();
                ChestUI.Hide();
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
                    JinventoryUI.Hide();
                    BackpackUI.Hide();
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
