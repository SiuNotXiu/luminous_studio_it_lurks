using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight_z_depth : MonoBehaviour
{
    [HideInInspector] private GameObject object_camera_main;

    private void OnValidate()
    {
        modify_z_depth();
    }
    private void Start()
    {
        modify_z_depth();
    }

    void modify_z_depth()
    {
        #region set z depth
        object_camera_main = GameObject.Find("camera_main_dont_change_name");
        //z + 1 is used for black circle
        if (object_camera_main != null)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y,
                object_camera_main.transform.position.z + 1);
        }
        else
        {
            Debug.Log("(can ignore) object_camera_main is null, cannot move the z depth of flashlight");
        }
        #endregion
    }
}
