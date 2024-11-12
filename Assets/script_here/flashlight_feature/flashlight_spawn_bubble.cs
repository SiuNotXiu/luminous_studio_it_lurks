using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight_spawn_bubble : MonoBehaviour
{
    //this script is used by many_bubble
    //to spawn bubble for mask

    [SerializeField] private GameObject object_bubble;
    [HideInInspector] private GameObject object_player;
    [HideInInspector] private GameObject object_zone_that_need_bubble;

    [HideInInspector] private GameObject object_many_bubble;
    [HideInInspector] private GameObject object_bubble_holder;
    [HideInInspector] private GameObject object_bubble_last_spawned;
    [HideInInspector] private Vector3 spawn_position;

    [SerializeField] private float x_left;
    [SerializeField] private float x_right;
    [SerializeField] private float y_down;
    [SerializeField] private float y_up;

    [SerializeField] private float x_gap;
    [SerializeField] private float y_gap;

    [HideInInspector] private int x_row = 0;
    [HideInInspector] private int y_column = 0;

    private void OnValidate()
    {
        spawn_bubble();
    }

    private void Start()
    {
        spawn_bubble();
    }

    void spawn_bubble()
    {
        if (object_many_bubble == null)
        {
            object_player = GameObject.Find("player_dont_change_name");
            if (object_player != null)//can ignore if null reference
            {
                object_zone_that_need_bubble = object_player.transform.
                    Find("black_square_solution").
                    Find("zone_that_need_bubble").gameObject;
            }

            object_many_bubble = GameObject.Find("many_bubble"); 
        }
        else
        {
            #region spawn bubble
            if (object_many_bubble.transform.childCount == 0)
            {
                object_bubble_holder = new GameObject("bubble_holder");
                object_bubble_holder.transform.parent = object_many_bubble.transform;
                object_bubble_holder.transform.position = object_many_bubble.transform.position;

                float bubble_spawned = 0;

                Vector2 size = object_zone_that_need_bubble.GetComponent<BoxCollider2D>().size;
                Vector2 offset = object_zone_that_need_bubble.GetComponent<BoxCollider2D>().offset;

                x_left = object_zone_that_need_bubble.transform.position.x  + offset.x - (size.x * object_zone_that_need_bubble.transform.localScale.x) / 2;
                x_right = object_zone_that_need_bubble.transform.position.x + offset.x + (size.x * object_zone_that_need_bubble.transform.localScale.x) / 2;
                y_down = object_zone_that_need_bubble.transform.position.y  + offset.y - (size.y * object_zone_that_need_bubble.transform.localScale.y) / 2;
                y_up = object_zone_that_need_bubble.transform.position.y    + offset.y + (size.y * object_zone_that_need_bubble.transform.localScale.y) / 2;
                x_gap = 1f;
                y_gap = 1f;
                x_row = (int)((x_right - x_left) / x_gap);
                y_column = (int)((y_up - y_down) / y_gap);
                for (float x = x_left; x < x_right; x += x_gap)
                {
                    for (float y = y_up; y > y_down; y -= y_gap)
                    {
                        spawn_position = new Vector3(x, y, object_many_bubble.transform.position.z);
                        object_bubble_last_spawned = Instantiate(object_bubble, spawn_position, Quaternion.identity, object_bubble_holder.transform);
                        object_bubble_last_spawned.name = "bubble_" + bubble_spawned;

                        #region initialization for object_bubble_last_spawned
                        object_bubble_last_spawned.GetComponent<flashlight_dissolve_bubble>().object_player = object_player;

                        object_bubble_last_spawned.GetComponent<bubble_movement>().object_player = object_player;
                        object_bubble_last_spawned.GetComponent<bubble_movement>().object_zone_that_need_bubble = object_zone_that_need_bubble;

                        object_bubble_last_spawned.GetComponent<bubble_movement>().x_row = x_row;//the first row will get 0
                        object_bubble_last_spawned.GetComponent<bubble_movement>().y_column = y_column;//the first row will get 0

                        object_bubble_last_spawned.GetComponent<bubble_movement>().x_gap = x_gap;
                        object_bubble_last_spawned.GetComponent<bubble_movement>().y_gap = y_gap;
                        #endregion

                        //counting
                        bubble_spawned++;
                    }
                }
            }
            #endregion
        }
    }
}
