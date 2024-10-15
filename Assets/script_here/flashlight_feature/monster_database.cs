using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_database : MonoBehaviour
{
    //this script is used by monster
    
    [SerializeField] public bool flashed = false;

    private bool flash = false;
    private float flashtimer = 0;
    private float flashtime = 2;

    private void Update()
    {
        if (flashed)
        {
            flashtimer += Time.deltaTime;
            if (flashtimer > flashtime)
            {
                die();
                flash = true;
                flashtimer = 0;
            }
           
            
        }
    }

    void die()
    {
        //Destroy(gameObject);
        Debug.Log("Flashed");
    }

    public bool GetFlashed()
    {
        Debug.Log("Flash function work");
        return flash;
    }
}
