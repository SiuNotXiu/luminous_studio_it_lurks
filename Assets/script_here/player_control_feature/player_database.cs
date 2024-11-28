using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class player_database : MonoBehaviour
{
    [HideInInspector] public static bool is_flashlight_on = false;
    [HideInInspector] public static bool in_safe_zone = false;
    [HideInInspector] public static bool dead = false;

    private void Update()
    {
       // Debug.Log("in_safe_zone > " + in_safe_zone);
    }
}
