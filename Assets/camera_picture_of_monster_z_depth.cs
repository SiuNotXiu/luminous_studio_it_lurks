using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_picture_of_monster_z_depth : MonoBehaviour
{
    //this script is used by camera_picture_of_monster
    //to ensure that the render don't involve the flashlight mask effect
    [SerializeField] private GameObject object_flashlight_mask;

    private void OnValidate()
    {
        if (object_flashlight_mask == null)
        {
            object_flashlight_mask = transform.parent.Find("animation").Find("arm_with_flashlight").Find("flashlight_mask").gameObject;
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             object_flashlight_mask.transform.position.z + 1);
        }
    }
}
