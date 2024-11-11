using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight_battery_blink : MonoBehaviour
{
    //this script is used by flashlight_mask
    //to change the material and create the visual flashlight blinking
    [HideInInspector] private Material material_3d_mask_material;
    [HideInInspector] private Material material_none;

    [HideInInspector] public static float battery_percentage = 0;
    [HideInInspector] public static bool flashlight_during_blink = false;
    private void Start()
    {
        material_3d_mask_material = gameObject.GetComponent<MeshRenderer>().material;
        material_none = null;
    }

    public void check_should_flashlight_blink(float battery_remaining, float battery_max)
    {
        //Debug.Log("checking blink");
        battery_percentage = battery_remaining / battery_max * 100;

        if (battery_percentage == Mathf.Clamp(battery_percentage,   2.5f,   2.5f    + 1) ||
            battery_percentage == Mathf.Clamp(battery_percentage,    5,      5       + 1) ||
            battery_percentage == Mathf.Clamp(battery_percentage,    10,     10      + 1) ||
            battery_percentage == Mathf.Clamp(battery_percentage,    30,     30      + 1) ||
            battery_percentage == Mathf.Clamp(battery_percentage,    60,     60      + 1))
        {
            /*if (gameObject.GetComponent<MeshRenderer>().material != material_none)
                gameObject.GetComponent<MeshRenderer>().material = material_none;*/
            flashlight_during_blink = true;
        }
        else
        {
            /*if (gameObject.GetComponent<MeshRenderer>().material != material_3d_mask_material)
                gameObject.GetComponent<MeshRenderer>().material = material_3d_mask_material;*/
            flashlight_during_blink = false;
        }
    }
}
