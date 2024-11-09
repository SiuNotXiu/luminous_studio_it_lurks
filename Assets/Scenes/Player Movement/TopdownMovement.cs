using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownMovement : MonoBehaviour
{
    public static float moveSpeed = 10f;
    public Rigidbody2D rb2d;
    
    private Vector2 moveInput;

    [HideInInspector] public static float multiplier_1300_mah = 0.75f;

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

    #region perks equip
    public static void equip_20k_lumen_bulb()
    {
        moveSpeed *= multiplier_1300_mah;
    }
    #endregion
    #region perks remove
    public static void remove_20k_lumen_bulb()
    {
        moveSpeed /= multiplier_1300_mah;
    }
    #endregion
}