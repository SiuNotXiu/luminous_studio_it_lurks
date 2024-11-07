using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_display_icon : MonoBehaviour
{
    [HideInInspector] private GameObject object_big_map_icon;

    private void Start()
    {
        object_big_map_icon = transform.Find("big_map_icon").gameObject;
    }

    public void display_icon_on_map()
    {
        object_big_map_icon.SetActive(true);
    }
}
