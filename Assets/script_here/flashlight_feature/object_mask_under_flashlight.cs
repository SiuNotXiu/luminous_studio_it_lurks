using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_mask_under_flashlight : MonoBehaviour
{
    void Start()//never use onValidate, dont ask why
    {
        GetComponent<SpriteRenderer>().material.renderQueue = 3002;
    }
}
