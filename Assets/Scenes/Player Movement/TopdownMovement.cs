using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Rigidbody2D rb2d;
    
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public void ChangeSpeed(float p_speed)
    {
        moveSpeed = p_speed;
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with > " + collision.gameObject.name);
    }
}