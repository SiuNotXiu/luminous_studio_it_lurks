using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_mask_under_flashlight : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().material.renderQueue = 3002;
    }
}
