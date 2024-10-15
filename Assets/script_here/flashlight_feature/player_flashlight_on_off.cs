using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_flashlight_on_off : MonoBehaviour
{
    #region script finding
    [HideInInspector] private player_database script_player_database;
    #endregion

    void Start()
    {
        script_player_database = GetComponent<player_database>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            script_player_database.is_flashlight_on = !script_player_database.is_flashlight_on;
        }
    }
}
