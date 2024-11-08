using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_flashlight_direction : MonoBehaviour
{
    //this script is used by player
    [Tooltip("gameobject will find")]
    [SerializeField] private flashlight_fov_wall_mask script_fov_mask;
    [HideInInspector] private Vector3 mouse_position;
    [HideInInspector] private Vector3 aim_direction;

    void OnValidate()
    {
        //script_fov_mask = gameObject.transform.Find("flashlight_mask_dont_change_name").gameObject.GetComponent<flashlight_fov_wall_mask>();
    }

    void Update()
    {
        mouse_position = get_mouse_position();
        aim_direction = (mouse_position - transform.position).normalized;

        /*if (script_fov_mask != null)
        {
            script_fov_mask.set_origin(transform.position);
            script_fov_mask.set_aim_direction(aim_direction);
        }
        else
        {
            Debug.Log("script_fov_mask is null");
        }*/
        flashlight_fov_wall_mask.angle = flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction) + flashlight_fov_wall_mask.fov / 2.0f;
        flashlight_fov_wall_mask.player_position = transform.position;
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
