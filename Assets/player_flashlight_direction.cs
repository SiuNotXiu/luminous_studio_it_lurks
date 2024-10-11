using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_flashlight_direction : MonoBehaviour
{
    [HideInInspector] private flashlight_fov_wall_mask script_fov_mask;
    [HideInInspector] private flashlight_fov_wall_mask script_fov_visible;
    [HideInInspector] private Vector3 mouse_position;
    [HideInInspector] private Vector3 aim_direction;

    void Start()
    {
        script_fov_mask = gameObject.transform.Find("flashlight_mask(dont_change_name)").gameObject.GetComponent<flashlight_fov_wall_mask>();
        script_fov_visible = gameObject.transform.Find("flashlight_visible(dont_change_name)").gameObject.GetComponent<flashlight_fov_wall_mask>();
    }

    // Update is called once per frame
    void Update()
    {
        mouse_position = get_mouse_position();
        aim_direction = (mouse_position - transform.position).normalized;

        //position of origin and mouse direction based on player
        script_fov_mask.set_origin(transform.position, transform.position);
        script_fov_mask.set_aim_direction(aim_direction);

        script_fov_visible.set_origin(transform.position, transform.position);
        script_fov_visible.set_aim_direction(aim_direction);
    }

    #region general functions
    Vector3 get_mouse_position()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f; // Set z to 0 for 2D
        return worldPosition;
    }
    #endregion
}
