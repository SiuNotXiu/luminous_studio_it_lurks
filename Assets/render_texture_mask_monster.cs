using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class render_texture_mask_monster : MonoBehaviour
{
    //this script is used by camera_mask_for_monster_in_range
    //to output render texture for a mask

    [SerializeField] private List<GameObject> object_shape_for_mask;
    [SerializeField] private RenderTexture mask_for_monster_in_range;
    [HideInInspector] private List<int> record_layer = new List<int>();
    [HideInInspector] private int layer_mask_for_monster_in_range;

    private void Start()
    {
        layer_mask_for_monster_in_range = LayerMask.NameToLayer("mask_for_monster_in_range");
        GetComponent<Camera>().targetTexture = mask_for_monster_in_range;
    }
    private void LateUpdate()
    {
        take_picture();
    }
    private void Update()
    {
        take_picture();
    }

    void take_picture()
    {
        //set to specific layer
        for (int i = 0; i < object_shape_for_mask.Count; i++)
        {
            record_layer.Add(object_shape_for_mask[i].layer);
            //Debug.Log(object_shape_for_mask[i].name + " layer 1 is " + object_shape_for_mask[i].layer);
            object_shape_for_mask[i].layer = layer_mask_for_monster_in_range;
            //Debug.Log(object_shape_for_mask[i].name + " layer 2 is " + object_shape_for_mask[i].layer);
        }

        //take a photo
        GetComponent<Camera>().cullingMask = 1 << layer_mask_for_monster_in_range;
        GetComponent<Camera>().Render();

        //go back to original layer that they belongs to
        for (int i = 0; i < object_shape_for_mask.Count; i++)
        {
            object_shape_for_mask[i].layer = record_layer[i];
        }
        record_layer.Clear();
    }
}
