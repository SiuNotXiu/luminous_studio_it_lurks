using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight_z_depth : MonoBehaviour
{
    [SerializeField] private GameObject object_camera_main;
    [SerializeField] private GameObject object_flashlight_dim_filter;

    private void OnValidate()
    {
        if (object_flashlight_dim_filter == null)
            object_flashlight_dim_filter = GameObject.Find("flashlight_dim_filter");
        if (object_flashlight_dim_filter == null)
            Debug.Log("object_flashlight_dim_filter == null");

        if (object_camera_main == null)
            object_camera_main = GameObject.Find("camera_main_dont_change_name");
        if (object_camera_main == null)
            Debug.Log("object_camera_main == null");

        modify_z_depth();
    }
    private void Update()
    {
        modify_z_depth();
    }

    public void modify_z_depth()
    {
        #region set z depth
        //z + 1 is used for black circle
        if (object_camera_main != null && object_flashlight_dim_filter != null)
            {
            if (gameObject.name == "flashlight_mask" ||
                gameObject.name == "flashlight_got_monster_damage")
            {
                transform.position = new Vector3(transform.position.x,
                                                 transform.position.y,
                                                 object_camera_main.transform.position.z + 10);
            }
            else
            {
                //other flashlight must cover dim filter
                /*Debug.Log("object_camera_main.transform.position.z > " + object_camera_main.transform.position.z);
                Debug.Log("object_flashlight_dim_filter.transform.position.z > " + object_flashlight_dim_filter.transform.position.z);
                Debug.Log("position.z > " + (object_camera_main.transform.position.z + object_flashlight_dim_filter.transform.position.z) / 2);*/
                transform.position = new Vector3(transform.position.x,
                                                 transform.position.y,
                                                 (object_camera_main.transform.position.z + object_flashlight_dim_filter.transform.position.z) / 2);
            }
        }
        else
        {
            //Debug.Log("(can ignore) object_camera_main or object_flashlight_dim_filter is null, cannot move the z depth of flashlight");
        }
        #endregion
    }
}
