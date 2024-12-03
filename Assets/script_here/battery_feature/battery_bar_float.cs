using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battery_bar_float : MonoBehaviour
{
    [HideInInspector] public static float battery_remaining = 40.0f;
    [SerializeField] public float display_battery_remaining;
    [SerializeField] public float battery_remaining_percentage = 1.0f;
    [SerializeField] public float alpha;
    [HideInInspector] public static float battery_max = 40f;
    //the greater this battery_duration_multiplier is, the longer battery last
    [SerializeField] public Image battery_green;
    public enum which_battery_used
    {
        battery_normal,
        battery_1300_mah
    }
    [HideInInspector] public static which_battery_used previous_battery = which_battery_used.battery_normal;
    #region perks 1300mah
    [SerializeField] public static bool using_1300_mah_casing = false;
    [HideInInspector] public static float multiplier_1300_mah = 1.5f;
    #endregion
    #region perks 20k lumen bulb
    [HideInInspector] public static float multiplier_20k_lumen_bulb = 1.0f / 1.5f;
    #endregion

    [SerializeField] private flashlight_battery_blink script_flashlight_battery_blink;

    //dimmer visuals
    [SerializeField] private GameObject object_dim_filter;

    private void OnValidate()
    {
        #region initialize game object
        //if (battery_green == null)
        //    battery_green = GameObject.Find("canvas_battery_bar").transform.Find("green").gameObject.GetComponent<Image>();
        if (battery_green == null)
            Debug.Log("battery_green == null");

        if (script_flashlight_battery_blink == null)
            script_flashlight_battery_blink = GameObject.Find("arm_with_flashlight").transform.Find("flashlight_mask").GetComponent<flashlight_battery_blink>();
        if (script_flashlight_battery_blink == null)
            Debug.Log("script_flashlight_battery_blink == null");
        
        if (object_dim_filter == null)
            object_dim_filter = GameObject.Find("flashlight_dim_filter");
        if (object_dim_filter == null)
            Debug.Log("object_dim_filter == null");
        #endregion
    }
    private void Start()
    {
        change_dim_filter_alpha();
    }
    void Update()
    {
        //Debug.Log("battery_remaining > " + battery_remaining);
        //Debug.Log("battery_max > " + battery_max);
        display_battery_remaining = battery_remaining;
        if (player_database.is_flashlight_on == true)
        {
            flashlight_on_battery_consumption();
            battery_bar_display();
            change_dim_filter_alpha();
        }
        /*if (Input.GetKeyDown(KeyCode.R))//temporary reload, later change to chermin use battery in journal
        {
            reload_battery(which_battery_used.battery_normal);
            battery_bar_display();
            change_dim_filter_alpha();
        }*/
    }

    void flashlight_on_battery_consumption()
    {
        battery_remaining -= Time.deltaTime;    //constantly reduce the same one
        battery_remaining_percentage = battery_remaining / battery_max;
    }
    void battery_bar_display()
    {
        #region battery changed, check animation
        if (script_flashlight_battery_blink != null)
            script_flashlight_battery_blink.check_should_flashlight_blink(battery_remaining, battery_max);

        /*if (battery_remaining >= 0)
        {
            //visual
            if (battery_green != null)
                battery_green.fillAmount = battery_remaining_percentage;
        }*/
        if (battery_remaining <= 0)//only need to check this if flashlight is on
        {
            battery_remaining = 0;
            player_database.is_flashlight_on = false;
        }
        #endregion
    }
    
    public static bool reload_battery(which_battery_used which_battery)
    {
        if (which_battery == which_battery_used.battery_normal)
        {
            //Debug.Log("normal");
            if (previous_battery == which_battery_used.battery_1300_mah)
            {
                //just now also using 1300
                //which means just now already multiplied the battery_max
                previous_battery = which_battery_used.battery_normal;//1300(previous) to normal(current)
                battery_max = battery_max * multiplier_1300_mah;
            }
            battery_remaining = battery_max;
            return true;
        }
        else if (which_battery == which_battery_used.battery_1300_mah)
        {
            //already confirm that only 1300 use success will reach here
            //Debug.Log("1300");
            //Debug.Log("using_1300_mah_casing > " + using_1300_mah_casing);
            if (using_1300_mah_casing == true)
            {
                if (previous_battery == which_battery_used.battery_normal)
                {
                    //just now also using 1300
                    //which means just now already multiplied the battery_max
                    //Debug.Log("updating battery max");
                    previous_battery = which_battery_used.battery_1300_mah;//1300(current) to normal(previous)
                    battery_max *= multiplier_1300_mah;
                }
                battery_remaining = battery_max;
                return true;
            }
            return false;
        }
        else
            return false;
    }
    
    void change_dim_filter_alpha()
    {
        // y = 1 - e^(-kx)
        float x = battery_remaining_percentage;
        float k = 3;
        alpha = Mathf.Exp(-k * x);
        /*object_dim_filter.GetComponent<SpriteRenderer>().color = new Color(object_dim_filter.GetComponent<SpriteRenderer>().color.r,
            object_dim_filter.GetComponent<SpriteRenderer>().color.g,
            object_dim_filter.GetComponent<SpriteRenderer>().color.b,
            alpha);*/
        if (alpha < 0)
            alpha = 0;
        /*if (alpha < 0.3f && alpha > 0)
            alpha = 0.3f;*/
        //object_dim_filter.GetComponent<SpriteRenderer>().material.SetFloat("_alpha", alpha);
        if (object_dim_filter == null)
            object_dim_filter = GameObject.Find("flashlight_dim_filter");
        object_dim_filter.GetComponent<MeshRenderer>().material.SetFloat("_alpha", alpha);
    }

    #region perks equip
    public static void equip_1300_mah_casing()
    {
        using_1300_mah_casing = true;
        //longer duration battery
        //battery_max *= multiplier_1300_mah;//20 multiply 1.5
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
