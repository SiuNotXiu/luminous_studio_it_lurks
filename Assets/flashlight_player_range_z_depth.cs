using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight_player_range_z_depth : MonoBehaviour
{
    //this script is only used by circle_for_mask_monster_in_range
    //to ensure that it's below flashlight_mask_for_monster_in_range

    [SerializeField] private GameObject object_flashlight_mask_for_monster_in_range;
    [HideInInspector] private Vector3 target_position;

    private void Start()
    {
        if (object_flashlight_mask_for_monster_in_range == null)
        {
            object_flashlight_mask_for_monster_in_range = GameObject.Find("flashlight_mask_for_monster_in_range");
        }
    }

    private void Update()
    {
        float add_z_depth = 0.01f;
        if (gameObject.name == "black_circle_for_mask_monster_in_range")
        {
            add_z_depth = 0.01f + 0.01f;
        }
        else
        {
            add_z_depth = 0.01f;
        }
        target_position = new Vector3(transform.position.x,
                                      transform.position.y,
                                      object_flashlight_mask_for_monster_in_range.transform.position.z + add_z_depth);
        if (transform.position != target_position)
        {
            transform.position = target_position;
        }
    }
}
