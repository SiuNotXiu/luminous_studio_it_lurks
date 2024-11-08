using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_mask_under_flashlight : MonoBehaviour
{
    //this script is used for object that should be invisible if overlapped with flashlight

    void Start()//never use onValidate, dont ask why
    {
        //as long as greater than 3001, will have the masking effect
        GetComponent<SpriteRenderer>().material.renderQueue = 3002;
        /*if (gameObject.name.StartsWith("bubble"))
            GetComponent<SpriteRenderer>().material.renderQueue = 3002;
        else
            GetComponent<SpriteRenderer>().material.renderQueue = 3003;*/
    }
}
