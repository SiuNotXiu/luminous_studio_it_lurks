using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battery_bar_float : MonoBehaviour
{
    [SerializeField] public static float battery_remaining = 20.0f;
    [SerializeField] public float display_battery_remaining;
    [HideInInspector] public static float battery_max;
    [HideInInspector] public static float battery_max_normal = 20.0f;
    [HideInInspector] public static float battery_max_1300_mah = 20.0f;
    //the greater this battery_duration_multiplier is, the longer battery last
    [SerializeField] public Image battery_green;

    #region perks 1300mah
    [HideInInspector] public static bool using_1300_mah_casing = false;
    [HideInInspector] public static float multiplier_1300_mah = 1.5f;
    #endregion
    #region perks 20k lumen bulb
    [HideInInspector] public static float multiplier_20k_lumen_bulb = 1.0f / 1.5f;
    #endregion

    [HideInInspector] private flashlight_battery_blink script_flashlight_battery_blink;

    void Start()
    {
        script_flashlight_battery_blink = transform.Find("flashlight_mask").GetComponent<flashlight_battery_blink>();
        battery_max = battery_max_normal;
    }

    void Update()
    {
        display_battery_remaining = battery_remaining;
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
            battery_remaining = battery_max_normal;
        }
        if (battery_remaining > battery_max)
        {
            battery_remaining = battery_max;
        }
    }

    #region perks equip
    public static void equip_1300_mah_casing()
    {
        using_1300_mah_casing = true;
        //longer duration battery
        battery_max *= multiplier_1300_mah;//20 multiply 1.5
    }
    public static void equip_20k_lumen_bulb()
    {
        //shorter duration battery
        battery_remaining *= multiplier_20k_lumen_bulb;//10 multiply 0.66
        battery_max *= multiplier_20k_lumen_bulb;//20 multiply 0.66
    }
    #endregion

    #region perks remove
    public static void remove_1300_mah_casing()
    {
        using_1300_mah_casing = false;
        battery_max /= multiplier_1300_mah;
    }
    public static void remove_20k_lumen_bulb()
    {
        //battery percentage need to remain
        //so remaining need to multiply back also
        battery_remaining /= multiplier_20k_lumen_bulb;
        battery_max /= multiplier_20k_lumen_bulb;
    }
    #endregion
}
