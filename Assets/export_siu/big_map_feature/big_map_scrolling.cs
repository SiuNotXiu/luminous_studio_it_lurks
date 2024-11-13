using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class big_map_scrolling : MonoBehaviour
{
    //this script is used by canvas
    //to modify the scale of big_map_background
    //by mouse scrolling
    //scale recovery is in <trigger_map_ui>

    [HideInInspector] private GameObject object_map_and_icon;

    [HideInInspector] private Vector2 obmb_initial_scale;
    [HideInInspector] private Vector2 obmb_zoom_in_scale;
    [HideInInspector] private Vector2 obmb_zoom_out_scale;

    [HideInInspector] private float zoom_in_percentage = 0.2f;
    [HideInInspector] private float zoom_out_percentage = 0.2f;

    [SerializeField] private float max_zoom_in_count;
    [SerializeField] public float current_zoom_count = 0f;

    private void OnValidate()
    {
        if (object_map_and_icon == null)
            object_map_and_icon = transform.Find("big_map").Find("map_and_icon").gameObject;
    }
    private void Start()
    {
        #region find_initial_scale
        obmb_initial_scale = object_map_and_icon.transform.localScale;
        obmb_zoom_in_scale = new Vector2(obmb_initial_scale.x * zoom_in_percentage,
                                         obmb_initial_scale.y * zoom_in_percentage);
        obmb_zoom_out_scale = new Vector2(obmb_initial_scale.x * zoom_out_percentage * (-1),
                                          obmb_initial_scale.y * zoom_out_percentage * (-1));

        max_zoom_in_count = Mathf.Floor(1 / zoom_in_percentage) * 3;
        #endregion
    }
    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)//positive for up, negative for down
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                //scrolling up
                if (current_zoom_count < max_zoom_in_count)
                {
                    //zoom in value around 5, current_zoom_in should be positive
                    current_zoom_count++;
                    object_map_and_icon.transform.localScale = new Vector2(object_map_and_icon.transform.localScale.x + obmb_zoom_in_scale.x,
                                                                                 object_map_and_icon.transform.localScale.y + obmb_zoom_in_scale.y);
                }
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                //scrolling down
                if (current_zoom_count > 0)//control the minimum zoom out
                {
                    //zoom in value around 5, current_zoom_in should be negative
                    current_zoom_count--;
                    object_map_and_icon.transform.localScale = new Vector2(object_map_and_icon.transform.localScale.x + obmb_zoom_out_scale.x,
                                                                             object_map_and_icon.transform.localScale.y + obmb_zoom_out_scale.y);
                }
            }
        }
    }


}
