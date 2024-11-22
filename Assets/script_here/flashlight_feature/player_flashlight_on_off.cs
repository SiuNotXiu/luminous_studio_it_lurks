using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_flashlight_on_off : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(InventoryController.JournalOpen && !trigger_map_ui.Map_Is_Open)
            {
                player_database.is_flashlight_on = !player_database.is_flashlight_on;
                flashlightSFX();
            }
            
        }
    }

    #region flashlightONOFf
    private void flashlightSFX()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXPlayerBehave.Instance.Flashlight, Audio.Instance.playerFlashlight);
        }
    }
    #endregion
}
