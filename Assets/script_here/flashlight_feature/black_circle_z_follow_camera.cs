using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class black_circle_z_follow_camera : MonoBehaviour
{
    [HideInInspector] private GameObject object_camera_main;

    // Update is called once per frame
    void OnValidate()
    {
        object_camera_main = GameObject.Find("camera_main_dont_change_name");
        if (object_camera_main != null)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y,
                object_camera_main.transform.position.z + 2);
        }
        else
        {
            Debug.Log("(can ignore) object_camera_main is null, cannot move the z depth of flashlight");
        }
    }
}
