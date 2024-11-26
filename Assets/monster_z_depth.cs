using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_z_depth : MonoBehaviour
{
    [SerializeField] private GameObject object_flashlight_mask;
    [SerializeField] private GameObject object_camera_picture_for_monster;

    private void OnValidate()
    {
        /*if (object_flashlight_mask == null)
            GameObject.Find("flashlight_mask");
        if (object_camera_picture_for_monster == null)
            GameObject.Find("camera_picture_for_monster");
        if (object_flashlight_mask.transform.position.z + 1 > object_camera_picture_for_monster.transform.position.z)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y,
                object_flashlight_mask.transform.position.z + 1);
        }
        else
        {
            Debug.Log("z layer is still wrong");
        }*/
    }

    private void Start()
    {
        
    }
}
