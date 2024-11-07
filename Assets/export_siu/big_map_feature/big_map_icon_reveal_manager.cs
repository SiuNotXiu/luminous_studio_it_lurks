using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class big_map_icon_reveal_manager : MonoBehaviour
{
    [SerializeField] private GameObject[] object_landmark_with_scrap_paper;

    public void call_this_after_scrap_paper_taken(bool[] scrap_paper_revealed)//any script can trigger this event
    {
        for (int i = 0; i < scrap_paper_revealed.Length; i++)
        {
            if (scrap_paper_revealed[i] == true)
            {
                if (object_landmark_with_scrap_paper[i].transform.Find("big_map_icon").gameObject.activeInHierarchy == false)
                {
                    Debug.Log("setting " + object_landmark_with_scrap_paper[i].name + " to active");
                    object_landmark_with_scrap_paper[i].transform.Find("big_map_icon").gameObject.SetActive(true);
                }
            }
        }
    }
}
