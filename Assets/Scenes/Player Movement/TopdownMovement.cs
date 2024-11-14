using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownMovement : MonoBehaviour
{
    public static float moveSpeed = 10f;
    [HideInInspector] public Rigidbody2D rb2d;
    
    private Vector2 moveInput;
    private bool facing_right = true;

    [HideInInspector] public static float multiplier_1300_mah = 0.75f;

    [HideInInspector] private Animator animator_mask;
    [HideInInspector] private Animator animator_normal;

    [HideInInspector] private GameObject object_sprite_sheet_mask;

    private void OnValidate()
    {
        if (gameObject.name == "player_dont_change_name")
        {
            if (animator_mask == null)
            {
                if (transform.Find("sprite_sheet_mask").gameObject != null)
                {
                    animator_mask = transform.Find("sprite_sheet_mask").gameObject.GetComponent<Animator>();
                    animator_normal = transform.Find("sprite_sheet_mask").transform.Find("sprite_sheet_normal").gameObject.GetComponent<Animator>();
                }
                else
                {
                    Debug.Log("cannot animator_mask find, maybe name changed");
                }
            }
        }
        else
        {
            Debug.Log("not player but using topdownmovement script");
        }
        if (object_sprite_sheet_mask == null)
            object_sprite_sheet_mask = transform.Find("sprite_sheet_mask").gameObject;
    }
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(InventoryController.JournalOpen )
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();
        }

        rb2d.velocity = moveInput * moveSpeed;

        if (moveInput.x != 0)
        {
            animator_mask.Play("walk_right");
            animator_normal.Play("walk_right");
            if (moveInput.x > 0)
            {
                facing_right = true;
                object_sprite_sheet_mask.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (moveInput.x < 0)
            {
                facing_right = false;
                object_sprite_sheet_mask.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            animator_mask.Play("idle_right");
            animator_normal.Play("idle_right");
        }
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public void ChangeSpeed(float p_speed)
    {
        moveSpeed = p_speed;
        
    }

    public void playerFacing()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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