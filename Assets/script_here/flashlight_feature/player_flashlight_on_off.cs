using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_flashlight_on_off : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            player_database.is_flashlight_on = !player_database.is_flashlight_on;
        }
    }
}
