using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battery_bar_float : MonoBehaviour
{
    #region script finding
    [HideInInspector] private player_database script_player_database;
    #endregion
    [SerializeField] public float battery_remaining = 20.0f;
    [HideInInspector] public float battery_max = 20.0f;
    [SerializeField] public Image battery_green;

    void Start()
    {
        script_player_database = GetComponent<player_database>();
    }

    void Update()
    {
        if (script_player_database.is_flashlight_on == true)
        {
            battery_remaining -= Time.deltaTime;
        }
        if (battery_remaining <= 0)
        {
            battery_remaining = 0;
            script_player_database.is_flashlight_on = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            battery_remaining = battery_max;
        }
        battery_green.fillAmount = battery_remaining / battery_max;
    }
}
