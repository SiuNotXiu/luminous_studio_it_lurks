using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_flashlight_direction : MonoBehaviour
{
    //this script is used by player
    [Tooltip("gameobject will find")]
    //bones
    [SerializeField] private GameObject object_sprite_sheet_mask;
    [SerializeField] private GameObject object_sprite_sheet_normal;
    [SerializeField] private GameObject object_arm_with_flashlight;
    [SerializeField] private GameObject bones_head_mask;
    [SerializeField] private GameObject bones_head_normal;

    //directional
    [SerializeField] private GameObject object_flashlight_mask;

    [HideInInspector] private Vector3 mouse_position;
    [HideInInspector] private Vector3 aim_direction;

    //[SerializeField] private float z;
    //[SerializeField] private float output_z;
    void OnValidate()
    {
        //script_fov_mask = gameObject.transform.Find("flashlight_mask_dont_change_name").gameObject.GetComponent<flashlight_fov_wall_mask>();
        
        if (object_sprite_sheet_mask == null)
            object_sprite_sheet_mask = transform.Find("animation").Find("sprite_sheet_mask").gameObject;
        if (object_sprite_sheet_normal == null)
            object_sprite_sheet_normal = transform.Find("animation").Find("sprite_sheet_normal").gameObject;
        if (object_arm_with_flashlight == null)
            object_arm_with_flashlight = transform.Find("animation").Find("arm_with_flashlight").gameObject;

        if (bones_head_mask == null)
            bones_head_mask = object_sprite_sheet_mask.transform.Find("bone_1").Find("bone_2").Find("bone_3").gameObject;
        if (bones_head_normal == null)
            bones_head_normal = object_sprite_sheet_normal.transform.Find("bone_1").Find("bone_2").Find("bone_3").gameObject;

        if (object_flashlight_mask == null)
            object_flashlight_mask = object_arm_with_flashlight.transform.Find("flashlight_mask").gameObject;
    }

    void Update()
    {
        mouse_position = get_mouse_position();
        aim_direction = (mouse_position - object_arm_with_flashlight.transform.position).normalized;
        
        flashlight_fov_wall_mask.angle = flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction) + flashlight_fov_wall_mask.fov / 2;
        flashlight_fov_wall_mask.player_position = object_flashlight_mask.transform.position;

        //output_z = flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction);//when the cursor is at 3 o clock, the angle is 0, perfect

        //Debug.Log("direction > " + flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction));



        if (flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction) >= 90 && flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction) <= 270)
        {
            //facing left
            object_arm_with_flashlight.transform.rotation = Quaternion.Euler(0, 
                                                                             180,
                                                                             0 - (flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction) + 90));
            object_flashlight_mask.transform.rotation = Quaternion.Euler(0,
                                                                         180,
                                                                         0);
            //ensure can see arm
            object_arm_with_flashlight.transform.position = new Vector3(object_arm_with_flashlight.transform.position.x,
                                                                        object_arm_with_flashlight.transform.position.y,
                                                                        object_sprite_sheet_mask.transform.position.z - 1f);
            object_flashlight_mask.GetComponent<flashlight_z_depth>().modify_z_depth();
        }
        else
        {
            //facing right
            object_arm_with_flashlight.transform.rotation = Quaternion.Euler(0,
                                                                             0,
                                                                             flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction) + 90); // Rotate the arm to face the mouse (2D)
            object_flashlight_mask.transform.rotation = Quaternion.Euler(0,
                                                                         0,
                                                                         0);
            //ensure can see arm
            object_arm_with_flashlight.transform.position = new Vector3(object_arm_with_flashlight.transform.position.x,
                                                                        object_arm_with_flashlight.transform.position.y,
                                                                        object_sprite_sheet_mask.transform.position.z - 1f);
            object_flashlight_mask.GetComponent<flashlight_z_depth>().modify_z_depth();
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
