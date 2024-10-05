using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    [SerializeField] private UI_Journal_Page JinventoryUI;
    [SerializeField] private UI_Backpack_Page BackpackUI;

    private int inventorySize = 6;
    private void Start()
    {
        JinventoryUI.InitializeInventoryUI(inventorySize);

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (JinventoryUI.isActiveAndEnabled == false) 
            {
                JinventoryUI.Show();
                BackpackUI.Hide();
            }
            else
            {
                JinventoryUI.Hide();
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            if (BackpackUI.isActiveAndEnabled == false)
            {
                BackpackUI.Show();
                JinventoryUI.Hide();
            }
            else
            {
                BackpackUI.Hide();
            }
        }
    }
}
