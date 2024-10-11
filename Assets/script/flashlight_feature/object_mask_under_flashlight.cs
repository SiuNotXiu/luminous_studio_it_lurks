using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_mask_under_flashlight : MonoBehaviour
{
    //this script is used by every grayscale / outline object
    //to show make the grayscale invisible, monoschrome visible

    void Start()
    {
        GetComponent<SpriteRenderer>().material.renderQueue = 3002;
    }
}
