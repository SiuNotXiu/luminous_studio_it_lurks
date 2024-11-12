using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testing : MonoBehaviour
{
    public static float moveSpeed = 10f;
    [HideInInspector] public Rigidbody2D rb2d;

    private Vector2 moveInput;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
        rb2d.velocity = moveInput * moveSpeed;
    }
}
