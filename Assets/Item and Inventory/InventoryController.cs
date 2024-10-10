using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Journal_display journal_display;
    [SerializeField] private Button_display button_display;

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (journal_display.isActiveAndEnabled == false)
            {
                journal_display.Show();
                journal_display.ShowPanels();

                button_display.Show();
                button_display.ShowPanels();

            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {

                button_display.HidePanels();
                journal_display.HidePanels();
                journal_display.Hide();
                button_display.Hide();
            }
            else
            {
                button_display.HidePanels();
                journal_display.HidePanels();
                journal_display.Hide();
                button_display.Hide();
            }
        }
    }
}

