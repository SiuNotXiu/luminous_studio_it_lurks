using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class bubble_movement : MonoBehaviour
{
    [HideInInspector] private bool left_bubble = false;
    [HideInInspector] private bool right_bubble = false;
    [HideInInspector] private bool up_bubble = false;
    [HideInInspector] private bool down_bubble = false;

    [Tooltip("every individual bubble got it's own id")]
    [HideInInspector] public int x_row = 0;
    [Tooltip("every individual bubble got it's own id")]
    [HideInInspector] public int y_column = 0;

    [Tooltip("all bubble same")]
    [HideInInspector] public float x_gap = 0;
    [Tooltip("all bubble same")]
    [HideInInspector] public float y_gap = 0;

    [HideInInspector] private float x_left;
    [HideInInspector] private float x_right;
    [HideInInspector] private float y_down;
    [HideInInspector] private float y_up;

    [HideInInspector] public GameObject object_player;
    [HideInInspector] public GameObject object_zone_that_need_bubble;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "zone_that_need_bubble")
        {
            //Debug.Log(gameObject.name + " out of zone");
            Vector2 size = object_zone_that_need_bubble.GetComponent<BoxCollider2D>().size;
            Vector2 offset = object_zone_that_need_bubble.GetComponent<BoxCollider2D>().offset;
            x_left = object_zone_that_need_bubble.transform.position.x + offset.x - (size.x * object_zone_that_need_bubble.transform.localScale.x) / 2;
            x_right = object_zone_that_need_bubble.transform.position.x + offset.x + (size.x * object_zone_that_need_bubble.transform.localScale.x) / 2;
            y_down = object_zone_that_need_bubble.transform.position.y + offset.y - (size.y * object_zone_that_need_bubble.transform.localScale.y) / 2;
            y_up = object_zone_that_need_bubble.transform.position.y + offset.y + (size.y * object_zone_that_need_bubble.transform.localScale.y) / 2;

            Debug.Log("x_left > " + x_left);
            Debug.Log("x_right > " + x_right);
            Debug.Log(gameObject.name + " > " + transform.position);
            #region x movement
            if (transform.position.x < x_left)
            {
                Debug.Log(gameObject.name + " exit from left");
                transform.position = new Vector3(transform.position.x + x_row * x_gap,
                    transform.position.y,
                    transform.position.z);
            }
            if (transform.position.x > x_right)
            {
                Debug.Log(gameObject.name + " exit from right");
                transform.position = new Vector3(transform.position.x - x_row * x_gap,
                    transform.position.y,
                    transform.position.z);
            }
            #endregion
            #region y movement
            if (transform.position.y < y_down)
            {
                Debug.Log(gameObject.name + " exit from down");
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + y_column * y_gap,
                    transform.position.z);
            }
            if (transform.position.y > y_up)
            {
                Debug.Log(gameObject.name + " exit from up");
                transform.position = new Vector3(transform.position.x,
                    transform.position.y - y_column * y_gap,
                    transform.position.z);
            }
            #endregion
        }
    }
}
