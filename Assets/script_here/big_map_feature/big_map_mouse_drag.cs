using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class big_map_mouse_drag : MonoBehaviour
{
    //this script is used by canvas
    //to drag the map

    [SerializeField] private GameObject object_map_and_icon;
    [HideInInspector] private Vector2 clicked_point_delta_with_map;

    private void Update()
    {
        if (object_map_and_icon.activeInHierarchy == true)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    clicked_point_delta_with_map = object_map_and_icon.transform.position - Input.mousePosition;
                }
                object_map_and_icon.transform.position = new Vector2(Input.mousePosition.x + clicked_point_delta_with_map.x,
                                                                     Input.mousePosition.y + clicked_point_delta_with_map.y);
            }
        }
    }
}
