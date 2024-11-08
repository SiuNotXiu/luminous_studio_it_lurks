using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battery_bar_float : MonoBehaviour
{
    [SerializeField] public float battery_remaining = 20.0f;
    [HideInInspector] public float battery_max = 20.0f;
    [HideInInspector] public float battery_duration_multiplier = 1.0f;//the greater this value is, the longer battery last
    [SerializeField] public Image battery_green;

    [HideInInspector] private flashlight_battery_blink script_flashlight_battery_blink;

    void Start()
    {
        script_flashlight_battery_blink = transform.Find("flashlight_mask").GetComponent<flashlight_battery_blink>();
    }

    void Update()
    {
        if (player_database.is_flashlight_on == true)
        {
            battery_remaining -= Time.deltaTime * battery_duration_multiplier;

            //battery changed, check animation
            script_flashlight_battery_blink.check_should_flashlight_blink(battery_remaining, battery_max);

            if (battery_remaining >= 0)
            {
                //visual
                battery_green.fillAmount = battery_remaining / battery_max;
            }
            if (battery_remaining <= 0)//only need to check this if flashlight is on
            {
                battery_remaining = 0;
                player_database.is_flashlight_on = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))//temporary reload, later change to chermin use battery in journal
        {
            battery_remaining = battery_max;
        }
    }
}
