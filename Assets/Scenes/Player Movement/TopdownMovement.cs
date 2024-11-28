using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownMovement : MonoBehaviour
{
    public static float moveSpeed = 6f;
    [HideInInspector] public Rigidbody2D rb2d;
    private float speedboost = 8f;
    private float confield = 3f;
    private float oriSpeed = 6f;

    private Vector2 moveInput;
    private bool facing_right = true;

    [HideInInspector] public static float multiplier_1300_mah = 0.75f;

    [SerializeField] private GameObject object_animation;
    [SerializeField] private GameObject object_sprite_sheet_mask;
    [SerializeField] private GameObject object_sprite_sheet_normal;

    [SerializeField] private Animator animator_mask;
    [SerializeField] private Animator animator_normal;
    //prevent animation overlapping
    [SerializeField] private HealthEffects script_health_effects;

    //item boost (sorry gais our speed modifier will contra)
    private bool isBoostActive = false; //to prevent stacking
    private Coroutine speedBoostCoroutine;
    private const float adrenalineMultiplier = 1.5f;
    private const float adrenalineDuration = 5f;


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
        if (script_health_effects == null)
        {
            script_health_effects = GameObject.Find("HealthControll").GetComponent<HealthEffects>();
        }
    }
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(InventoryController.JournalOpen && EasterEgg.closingEgg && !trigger_map_ui.Map_Is_Open)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();//prevent diagonal move too fast
        }
        else
        {
            moveInput.x = 0f;
            moveInput.y = 0f;
        }
        playerFacing();
        rb2d.velocity = moveInput * moveSpeed;
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            Debug.Log("moveinput > " + moveInput);
            Debug.Log("playerFacing().normalized > " + playerFacing().normalized);
            Debug.Log("dot product > " + Vector2.Dot(moveInput, playerFacing().normalized));
            // Play the "walk_right" animation regardless of direction, speed will handle direction
            if (Vector2.Dot(moveInput, playerFacing().normalized) >= 0)
            {
                //Debug.Log("normal");
                if (script_health_effects.currentHp > 0)
                {
                    animator_mask.Play("walk_right");
                    animator_normal.Play("walk_right");
                }
            }
            else if (Vector2.Dot(moveInput, playerFacing().normalized) < 0)
            {
                //going right, facing left
                //Debug.Log("moving backwards");
                moveInput = moveInput / 2;
                rb2d.velocity = moveInput * moveSpeed;
                if (script_health_effects.currentHp > 0)
                {
                    animator_mask.Play("walk_backwards");
                    animator_normal.Play("walk_backwards");
                }
            }
        }
        else
        {
            if (script_health_effects.currentHp > 0)
            {
                animator_mask.Play("idle_right");
                animator_normal.Play("idle_right");
            }
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

    public void ConfieldSpeed()
    {
        moveSpeed = confield;
    }

    public void OriginalSpeed()
    {
        moveSpeed = oriSpeed;
    }

    public void SpeedBoost()
    {
        moveSpeed = speedboost;
    }

    public Vector3 playerFacing()
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
        Vector3 mouse_direction = mousePosition - transform.position;
        return mouse_direction;
    }

    public void UseAdrenaline()
    {
        if (isBoostActive && speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine); //reset timer if active
        }

        speedBoostCoroutine = StartCoroutine(AdrenalineSpeedBoost());
    }

    private IEnumerator AdrenalineSpeedBoost()
    {
        isBoostActive = true;
        float boostedSpeed = oriSpeed * adrenalineMultiplier;
        moveSpeed = boostedSpeed;

        yield return new WaitForSeconds(adrenalineDuration);

        moveSpeed = oriSpeed;
        isBoostActive = false;
        speedBoostCoroutine = null;
    }

    #region Sound Effect


    #endregion


}