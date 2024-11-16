using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_mask_under_flashlight : MonoBehaviour
{
    //this script is used for object that should be invisible if overlapped with flashlight

    void Start()//never use onValidate, dont ask why
    {
        #region define which object need
        if (gameObject.name == "sprite_sheet_mask")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name != "bone_1")//if not bone
                {
                    if (transform.GetChild(i).gameObject.GetComponent<object_mask_under_flashlight>() == null)
                    {
                        //dont have this script
                        transform.GetChild(i).gameObject.AddComponent<object_mask_under_flashlight>();
                    }
                }
            }
        }
        #endregion
        //as long as greater than 3001, will have the masking effect
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().material.renderQueue = 3002;
        }
        else
        {
            //Debug.Log(gameObject.name + " don't have sprite renderer");
        }
        /*if (gameObject.name.StartsWith("bubble"))
            GetComponent<SpriteRenderer>().material.renderQueue = 3002;
        else
            GetComponent<SpriteRenderer>().material.renderQueue = 3003;*/
    }
}
