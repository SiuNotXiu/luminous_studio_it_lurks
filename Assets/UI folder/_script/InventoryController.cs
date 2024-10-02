using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private UI_Inventory_Page inventoryUI;

    private int inventorySize = 6;
    private void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryUI.isActiveAndEnabled == false) 
            {
                inventoryUI.Show();
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }
}
