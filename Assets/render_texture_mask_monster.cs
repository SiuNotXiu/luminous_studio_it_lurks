using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class render_texture_mask_monster : MonoBehaviour
{
    //this script is used by camera_mask_for_monster_in_range
    //to output render texture for a mask

    [SerializeField] private List<GameObject> object_shape_for_mask;
    [SerializeField] private RenderTexture mask_for_monster_in_range;
    [HideInInspector] private List<int> record_layer;
    [HideInInspector] private int layer_mask_for_monster_in_range;

    private void Start()
    {
        layer_mask_for_monster_in_range = LayerMask.NameToLayer("mask_for_monster_in_range");
    }
    private void LateUpdate()
    {
        //set to specific layer
        foreach (var shape in object_shape_for_mask)
        {
            record_layer.Add(shape.layer);
            shape.layer = layer_mask_for_monster_in_range;
        }

        //take a photo
        gameObject.GetComponent<Camera>().Render();

        //go back to original layer that they belongs to
        for (int i = 0; i < object_shape_for_mask.Count; i++)
        {
            object_shape_for_mask[i].layer = record_layer[i];
        }
        record_layer.Clear();
    }
}
