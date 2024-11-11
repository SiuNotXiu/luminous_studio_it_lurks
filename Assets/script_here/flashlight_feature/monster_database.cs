using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class monster_database : MonoBehaviour
{
    //this script is used by monster

    [SerializeField] public bool flashed = false;

    private bool flash = false;
    public float flashtimer = 0;
    private float flashtime = 2;
    private bool flee = false;

    private void Update()
    {
        if (flashed)
        {
            flash = true;
            flashtimer += Time.deltaTime;
            if (flashtimer > flashtime)
            {
                //die();
                flee = true;
                flashtimer = 0;
            }


        }
        //because flashlight works as lateUpdate()
        flashed = false;
    }

    void die()
    {
        Destroy(gameObject);
    }

    public bool GetFlashed()
    {
        //Debug.Log("Flash function work");
        return flash;
    }

    public bool GetShine()
    {
        return flashed;
    }

    public void SetFlashed(bool p_flash)
    {
        flash = p_flash;
    }

    public bool GetFlee()
    {
        return flee;
    }

    public void SetFlee(bool p_flee)
    {
        flee = p_flee;
    }
}