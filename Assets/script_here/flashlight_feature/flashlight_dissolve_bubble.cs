using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class flashlight_dissolve_bubble : MonoBehaviour
{
    //this script is used by bubble
    //to control the dissolve animation related variable
    //from 1 to 0

    [SerializeField] private bool dissolving = false;
    [SerializeField] private bool flashed = false;
    [HideInInspector] private Material material_bubble_000;

    [HideInInspector] private Coroutine animation_coroutine;
    [HideInInspector] private bool updated_this_frame = false;
    [HideInInspector] private float time_passed = 0f;
    [HideInInspector] private float duration_until_end = 0.3f;
    [HideInInspector] private float alpha = 1.0f;

    [HideInInspector] public GameObject object_player;
    [SerializeField] private float distance_with_player;
    [HideInInspector] private bool bubble_in_circle_radius = false;

    private void Start()
    {
        material_bubble_000 = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        distance_with_player = Vector2.Distance(transform.position, object_player.transform.position);
        if (distance_with_player < 3f)
        {
            bubble_in_circle_radius = true;
        }
        else
        {
            bubble_in_circle_radius = false;
        }
        if (bubble_in_circle_radius == false)
        {
            if (dissolving == false)
            {
                if (alpha <= 0)//transparent, not opaque, invisible
                {
                    if (flashed == false)//not pointed
                    {
                        //animation of fade in, become visible
                        //Debug.Log("reseting time passed");
                        time_passed = 0f;
                        dissolving = true;
                        animation_coroutine = StartCoroutine(animation(false));
                    }
                }
                else if (alpha >= 1)
                {
                    //remain opaque
                    time_passed = 0f;
                }
            }
        }
        else
        {
            time_passed = duration_until_end;
            animation_coroutine = StartCoroutine(animation(true));
        }

        flashed = false;
    }

    public void dissolve_bubble()
    {
        flashed = true;
        if (dissolving == false)
        {
            //prevent multiple raycast calling this function repeatdly
            dissolving = true;

            animation_coroutine = StartCoroutine(animation(true));
        }
    }

    IEnumerator animation(bool true_for_dissolve)
    {
        while (true)
        {
            // Ensure deltaTime is only added once per frame
            #region time passed for percentage calculation
            if (!updated_this_frame)
            {
                if (time_passed < duration_until_end)
                {
                    time_passed += Time.deltaTime;
                }
                else
                {
                    time_passed = duration_until_end;
                }

                // Mark as updated for this frame
                updated_this_frame = true;

                // Reset flag at the end of the frame
                yield return new WaitForEndOfFrame();
                updated_this_frame = false;
            }
            #endregion

            // Calculate alpha based on time passed
            if (true_for_dissolve == true)
            {
                alpha = (duration_until_end - time_passed) / duration_until_end;
            }
            else if(true_for_dissolve == false)
            {
                alpha = time_passed / duration_until_end;
            }
            material_bubble_000.SetFloat("_dissolve_amount", alpha);

            if (true_for_dissolve == true)
            {
                // Stop the coroutine when alpha reaches 0
                if (alpha <= 0f)
                {
                    dissolving = false;
                    yield break; // End the coroutine
                }
            }
            else if (true_for_dissolve == false)
            {
                if (alpha >= 1f)
                {
                    dissolving = false;
                    yield break; // End the coroutine
                }
            }

            yield return null; // Wait until the next frame
        }
    }
}
