using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class black_circle_x_y_follow_player : MonoBehaviour
{
    [SerializeField] private GameObject object_player;

    private void OnValidate()
    {
        if (object_player != null)
        {
            transform.position = new Vector2(object_player.transform.position.x,
                                             object_player.transform.position.y);
        }
    }
}
