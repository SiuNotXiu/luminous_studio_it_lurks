using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class black_circle_x_y_follow_player : MonoBehaviour
{
    [SerializeField] private GameObject object_player;

    private void OnValidate()
    {
        if (object_player == null)
        {
            object_player = GameObject.Find("player_dont_change_name");
        }
        else
        {
            transform.position = new Vector3(object_player.transform.position.x,
                                             object_player.transform.position.y,
                                             transform.position.z);
        }
    }
}
