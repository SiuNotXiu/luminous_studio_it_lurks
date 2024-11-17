using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight_dim_z_depth : MonoBehaviour
{
    //this script is only used by flashlight dim
    //to ensure the filter always cover above flashlight_mask

    [SerializeField] private GameObject object_flashlight_mask;
    [SerializeField] private GameObject object_camera_main;

    [HideInInspector] private float distance_mask_and_camera;

    private void OnValidate()
    {
        if (object_flashlight_mask == null)
        {
            if (transform.parent.gameObject.name == "flashlight_mask")
                object_flashlight_mask = transform.parent.gameObject;
            else
                Debug.Log("alert, flashlight_dim_z_depth parent not flashlight_mask");
        }
        if (object_camera_main == null)
        {
            object_camera_main = GameObject.Find("camera_main_dont_change_name");
        }
        modify_z_depth();
    }
    private void Update()
    {
        modify_z_depth();
    }

    void modify_z_depth()
    {
        if (object_camera_main != null && object_flashlight_mask != null)
        {
            //flashlight_mask
            //flashlight_dim
            //campsite_flashlight
            //camera
            /*Debug.Log(gameObject.name + " is moved to " + (object_flashlight_mask.transform.position.z + object_camera_main.transform.position.z) / 2);
            Debug.Log(object_flashlight_mask.name + " position is " + object_flashlight_mask.transform.position.z);
            Debug.Log(object_camera_main.name + " position is " + object_camera_main.transform.position.z);*/
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                        gameObject.transform.position.y,
                                                        (object_flashlight_mask.transform.position.z + object_camera_main.transform.position.z) / 2);
        }
        else
        {
            Debug.Log("(can ignore) object_camera_main or object_flashlight_dim_filter is null, cannot move the z depth of flashlight");
        }
    }
}
