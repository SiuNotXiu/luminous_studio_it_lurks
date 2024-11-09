using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battery_bar_float : MonoBehaviour
{
    [SerializeField] public static float battery_remaining = 20.0f;
    [HideInInspector] public static float battery_max = 20.0f;
    //the greater this battery_duration_multiplier is, the longer battery last
    [HideInInspector] public static float battery_duration_multiplier = 1.0f;
    [SerializeField] public Image battery_green;

    [HideInInspector] public static bool using_1300_mah_casing = false;


    [HideInInspector] private flashlight_battery_blink script_flashlight_battery_blink;

    void Start()
    {
        script_flashlight_battery_blink = transform.Find("flashlight_mask").GetComponent<flashlight_battery_blink>();
    }

    void Update()
    {
        if (player_database.is_flashlight_on == true)
        {
            battery_remaining -= Time.deltaTime;    //constantly reduce the same one

            #region battery changed, check animation
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
            #endregion
        }
        if (Input.GetKeyDown(KeyCode.R))//temporary reload, later change to chermin use battery in journal
        {
            reload_battery("normal");
        }
    }

    public static void reload_battery(string type)
    {
        if (type == "normal")
        {
            battery_remaining = battery_max;
        }
    }
    #region perks equip
    public static void equip_1300_mah_casing()
    {
        using_1300_mah_casing = true;
    }
    #endregion

    #region perks remove
    public static void remove_1300_mah_casing()
    {
        using_1300_mah_casing = false;
    }
    #endregion
}
