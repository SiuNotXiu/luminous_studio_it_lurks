using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight_pop_bubble : MonoBehaviour
{
    //this script is used by bubble
    //to modify the alpha of bubble


    //reduce alpha every x seconds
    [HideInInspector] public bool flashed = false;
    [SerializeField] private float flashed_seconds = 0f;
    [HideInInspector] private float lower_alpha_if_flashed_this_seconds = 0.2f;
    //prevent raycast repeatdly trigger
    [HideInInspector] public bool bool_just_flashed = false;

    //recover alpha if not flased
    [SerializeField] private float stopped_flashed_seconds = 0f;
    [HideInInspector] private float recover_alpha_if_stop_flashed = 0.3f;
    [HideInInspector] private SpriteRenderer script_sprite_renderer;

    private void Start()
    {
        script_sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        check_alpha_recover();
        flashed = false;
        bool_just_flashed = false;
    }

    public void lower_the_alpha()
    {
        if (bool_just_flashed == false)
        {
            //one frame can only add delta time once, because too many raycast calling this function
            bool_just_flashed = true;

            flashed = true;
            flashed_seconds += Time.deltaTime;
            if (flashed_seconds > lower_alpha_if_flashed_this_seconds)
            {
                //flashed enough time
                flashed_seconds -= lower_alpha_if_flashed_this_seconds;
                Color col = script_sprite_renderer.GetComponent<SpriteRenderer>().color;
                col.a -= 0.3f;
                script_sprite_renderer.GetComponent<SpriteRenderer>().color = col;
            }
        }
    }

    void check_alpha_recover()
    {
        if (flashed == false)
        {
            stopped_flashed_seconds += Time.deltaTime;
            if (stopped_flashed_seconds > recover_alpha_if_stop_flashed)
            {
                stopped_flashed_seconds = 0;
                Color col = script_sprite_renderer.GetComponent<SpriteRenderer>().color;
                col.a = 1f;
                script_sprite_renderer.GetComponent<SpriteRenderer>().color = col;
            }
        }
        else
        {
            stopped_flashed_seconds = 0;
        }
    }
}
