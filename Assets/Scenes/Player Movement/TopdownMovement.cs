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

    [SerializeField] private GameObject object_animation;
    [SerializeField] private GameObject object_sprite_sheet_mask;
    [SerializeField] private GameObject object_sprite_sheet_normal;

    [SerializeField] private Animator animator_mask;
    [SerializeField] private Animator animator_normal;

    private void OnValidate()
    {
        if (object_animation == null)
            object_animation = transform.Find("animation").gameObject;
        if (object_sprite_sheet_mask == null)
            object_sprite_sheet_mask    = transform.Find("animation").Find("sprite_sheet_mask").gameObject;
        if (object_sprite_sheet_normal == null)
            object_sprite_sheet_normal  = transform.Find("animation").Find("sprite_sheet_normal").gameObject;
        if (animator_mask == null)
        {
            animator_mask = object_sprite_sheet_mask.GetComponent<Animator>();
            animator_normal = object_sprite_sheet_normal.GetComponent<Animator>();
        }
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
        playerFacing();
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            #region move backwards(mouse position) speed should be slower
            /*if ((facing_right && moveInput.x < 0) || (!facing_right && moveInput.x > 0))
            {
                // Moonwalk
                animator_mask.SetFloat("Speed", -1f); // Play animation in reverse
                animator_normal.SetFloat("Speed", -1f);
            }
            else
            {
                // Normal walk
                animator_mask.SetFloat("Speed", 1f); // Play animation normally
                animator_normal.SetFloat("Speed", 1f);
            }*/
            #endregion

            // Play the "walk_right" animation regardless of direction, speed will handle direction
            animator_mask.Play("walk_right");
            animator_normal.Play("walk_right");
        }
        else
        {
            animator_mask.SetFloat("Speed", 1f); //reset speed in case it was set to -1
            animator_normal.SetFloat("Speed", 1f);

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
        if (mousePosition.x < transform.position.x)
        {
            facing_right = false;
            object_animation.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            facing_right = true;
            object_animation.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
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