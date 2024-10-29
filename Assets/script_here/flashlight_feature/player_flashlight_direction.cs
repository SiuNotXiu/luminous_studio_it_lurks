using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_flashlight_direction : MonoBehaviour
{
    //this script is used by player

    /*[HideInInspector] private flashlight_fov_with_damage script_fov_mask;
    [HideInInspector] private flashlight_fov_with_damage script_fov_visible;*/
    [SerializeField] private flashlight_fov_wall_mask script_fov_mask;
    [SerializeField] private flashlight_fov_wall_mask script_fov_visible;
    [SerializeField] private Vector3 mouse_position;
    [SerializeField] private Vector3 aim_direction;
    [SerializeField] private Camera camera_for_calculation;

    void Start()
    {
        /*script_fov_mask   = gameObject.transform.Find("mask").gameObject.GetComponent<flashlight_fov_with_damage>();
        script_fov_visible  = gameObject.transform.Find("visible").gameObject.GetComponent<flashlight_fov_with_damage>();*/
        script_fov_mask = gameObject.transform.Find("flashlight_mask_dont_change_name").gameObject.GetComponent<flashlight_fov_wall_mask>();
        //script_fov_visible = gameObject.transform.Find("flashlight_visible_dont_change_name").gameObject.GetComponent<flashlight_fov_wall_mask>();

        camera_for_calculation = GameObject.Find("camera_for_calculation").GetComponent<Camera>();
    }

    void Update()
    {
        mouse_position = get_mouse_position();
        aim_direction = (mouse_position - transform.position).normalized;

        if (script_fov_mask != null)
        {
            script_fov_mask.set_origin(transform.position, transform.position);
            script_fov_mask.set_aim_direction(aim_direction);
        }

        if (script_fov_visible != null)
        {
            script_fov_visible.set_origin(transform.position, transform.position);
            script_fov_visible.set_aim_direction(aim_direction);
        }

    }

    #region general functions
    Vector3 get_mouse_position()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;       // Set z to 0 for 2D
        return worldPosition;
    }
    #endregion
}
