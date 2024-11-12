using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class player_flashlight_direction : MonoBehaviour
{
    //this script is used by player
    [Tooltip("gameobject will find")]
    [SerializeField] private GameObject object_arm_with_flashlight;
    [SerializeField] private GameObject object_flashlight_got_monster_damage;

    [HideInInspector] private Vector3 mouse_position;
    [HideInInspector] private Vector3 aim_direction;

    //[SerializeField] private float z;
    //[SerializeField] private float output_z;
    void OnValidate()
    {
        //script_fov_mask = gameObject.transform.Find("flashlight_mask_dont_change_name").gameObject.GetComponent<flashlight_fov_wall_mask>();
        if (object_arm_with_flashlight == null)
        {
            object_arm_with_flashlight = transform.Find("arm_with_flashlight").gameObject;
        }
        if (object_flashlight_got_monster_damage == null)
        {
            object_flashlight_got_monster_damage = object_arm_with_flashlight.transform.Find("flashlight_got_monster_damage").gameObject;
        }
    }

    void Update()
    {
        mouse_position = get_mouse_position();
        aim_direction = (mouse_position - object_arm_with_flashlight.transform.position).normalized;
        
        flashlight_fov_wall_mask.angle = flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction) + flashlight_fov_wall_mask.fov / 2;
        flashlight_fov_wall_mask.player_position = object_flashlight_got_monster_damage.transform.position;

        //output_z = flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction);//when the cursor is at 3 o clock, the angle is 0, perfect

        object_arm_with_flashlight.transform.rotation = Quaternion.Euler(0, 0, flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction) + 90); // Rotate the arm to face the mouse (2D)
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
