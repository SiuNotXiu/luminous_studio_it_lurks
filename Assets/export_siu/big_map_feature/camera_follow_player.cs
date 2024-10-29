using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow_player : MonoBehaviour
{
    //input
    [HideInInspector] private GameObject object_player;
    //calculation
    [HideInInspector] private Vector3 final_camera_position;

    void Start()
    {
        object_player = GameObject.Find("player");
    }

    void Update()
    {
        final_camera_position = object_player.transform.position;
        final_camera_position.z -= 10.0f;
        transform.position = final_camera_position;
    }
}
