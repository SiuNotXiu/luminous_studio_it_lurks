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

    //directional
    [SerializeField] private GameObject object_flashlight_mask;

    [HideInInspector] private Vector3 mouse_position;
    [HideInInspector] private Vector3 aim_direction;

    //offset based on editor
    [SerializeField] private float arm_initial_offset = 0.0f;

    //for physics after player die
    [SerializeField] private HealthEffects script_health_effects;
    private void OnValidate()
    {
        #region initialize game object
        if (object_sprite_sheet_mask == null)
            object_sprite_sheet_mask = transform.Find("animation").Find("sprite_sheet_mask").gameObject;
        if (object_sprite_sheet_mask == null)
            Debug.Log("object_sprite_sheet_mask == null");

        if (object_sprite_sheet_normal == null)
            object_sprite_sheet_normal = transform.Find("animation").Find("sprite_sheet_normal").gameObject;
        if (object_arm_with_flashlight == null)
            object_arm_with_flashlight = GameObject.Find("arm_with_flashlight").gameObject;

        if (object_flashlight_mask == null)
            object_flashlight_mask = object_arm_with_flashlight.transform.Find("flashlight_mask").gameObject;
        #endregion

        arm_initial_offset = object_arm_with_flashlight.transform.position.z - object_sprite_sheet_mask.transform.position.z;

        if (script_health_effects == null)
        {
            script_health_effects = GameObject.Find("HealthControll").GetComponent<HealthEffects>();
        }
    }
    void Update()
    {
        mouse_position = get_mouse_position();
        aim_direction = (mouse_position - object_arm_with_flashlight.transform.position).normalized;

        flashlight_fov_wall_mask.angle = flashlight_fov_wall_mask.get_angle_from_vector_float(aim_direction) + flashlight_fov_wall_mask.fov / 2;
        flashlight_fov_wall_mask.player_position = object_flashlight_mask.transform.position;

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
                                                                        object_sprite_sheet_mask.transform.position.z + arm_initial_offset);
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
                                                                        object_sprite_sheet_mask.transform.position.z + arm_initial_offset);
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
