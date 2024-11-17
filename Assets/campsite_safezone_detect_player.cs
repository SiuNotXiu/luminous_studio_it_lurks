using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class campsite_safezone_detect_player : MonoBehaviour
{
    //this script is used by campsite
    //to modify variables in player database when player collided

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player_dont_change_name")
        {
            player_database.in_safe_zone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player_dont_change_name")
        {
            player_database.in_safe_zone = false;
        }
    }
}
