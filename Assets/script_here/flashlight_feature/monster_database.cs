using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_database : MonoBehaviour
{
    //this script is used by monster

    [SerializeField] public bool flashed = false;

    private void Update()
    {
        if (flashed)
        {
            die();
        }
    }

    void die()
    {
        Destroy(gameObject);
    }
}
