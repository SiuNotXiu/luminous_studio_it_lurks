using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class big_map_icon_reveal_manager : MonoBehaviour
{
    [SerializeField] private GameObject[] object_cross;

    //chenjie: maybe here need to consider what kind id of paper appear what kind icon
    public void call_this_after_scrap_paper_taken(bool[] scrap_paper_revealed)//any script can trigger this event 
    {
        //the transform looks like
        //cross
        //  big_map_icon
        //  circle_next_scrap_paper
        //      big_map_icon
        for (int i = 0; i < scrap_paper_revealed.Length; i++)
        {
            if (scrap_paper_revealed[i] == true)
            {
                if (object_cross[i].transform.Find("big_map_icon").gameObject.activeInHierarchy == false)
                {
                    //this scrap_paper taken
                    //mark a cross on current scrap_paper
                    //Debug.Log(object_cross[i] + " > " + object_cross[i + 1]);
                    //object_cross[i].transform.Find("big_map_icon").GetComponent<map_display_icon>().display_icon_on_map();
                    object_cross[i].GetComponent<map_display_icon>().display_icon_on_map();
                    //reveal next target with circle
                    //object_cross[i + 1].transform.Find("circle_next_scrap_paper").Find("big_map_icon").gameObject.GetComponent<map_display_icon>().display_icon_on_map();
                    //Debug.Log("111 > " + object_cross[i + 1].name);
                    //Debug.Log("111 > " + object_cross[i + 1].transform.Find("circle_next_scrap_paper").gameObject.name);
                    object_cross[i + 1].transform.Find("circle_next_scrap_paper").GetComponent<map_display_icon>().display_icon_on_map();
                }

            }
        }
    }
}
