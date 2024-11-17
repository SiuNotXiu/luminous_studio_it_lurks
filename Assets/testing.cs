using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testing : MonoBehaviour
{
    public static float moveSpeed = 10f;
    public static testing instance;
    [HideInInspector] public Rigidbody2D rb2d;

    private Vector2 moveInput;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
        rb2d.velocity = moveInput * moveSpeed;
    }

    public IEnumerator Knockback(float knockbackDuration,float knockbackPower,Transform obj)
    {
        float timer = 0;

        while (knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = (obj.transform.position - this.transform.position).normalized;
            rb2d.AddForce(-direction * knockbackPower);
        }

        yield return 0;
    }
}
