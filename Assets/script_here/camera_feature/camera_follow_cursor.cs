using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow_cursor : MonoBehaviour
{
    [HideInInspector] private GameObject object_player;
    void Start()
    {
        object_player = GameObject.Find("player_dont_change_name");
    }

    // Update is called once per frame
    void Update()
    {
        float remember_this_float = object_player.transform.position.z - 100f;
        transform.position = (get_mouse_position() + object_player.transform.position) / 2;
        transform.position = new Vector3(transform.position.x,
            transform.position.y,
            remember_this_float);
    }
    Vector3 get_mouse_position()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f; // Set z to 0 for 2D
        return worldPosition;
    }
}
