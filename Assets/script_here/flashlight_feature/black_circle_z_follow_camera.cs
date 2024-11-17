using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class black_circle_z_follow_camera : MonoBehaviour
{
    [SerializeField] private GameObject object_flashlight_mask;

    // Update is called once per frame
    void OnValidate()
    {
        modify_z_depth();
    }

    private void Update()
    {
        modify_z_depth();
    }

    void modify_z_depth()
    {
        if (object_flashlight_mask == null)
            object_flashlight_mask = GameObject.Find("flashlight_mask");
        if (object_flashlight_mask != null)
        {
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             object_flashlight_mask.transform.position.z + 2);
        }
        else
        {
            Debug.Log("(can ignore) object_flashlight_dim_filter is null, cannot move the z depth of flashlight");
        }
    }
}
