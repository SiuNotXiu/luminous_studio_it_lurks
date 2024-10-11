using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    [HideInInspector] private float speed = 8f;

    [HideInInspector] private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //if (can_move)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) == false)
            {
                rb.velocity = new Vector2(rb.velocity.x, speed);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W) == false)
            {
                rb.velocity = new Vector2(rb.velocity.x, speed * (-1));
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) == false)
            {
                rb.velocity = new Vector2(speed * (-1), rb.velocity.y);
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A) == false)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
