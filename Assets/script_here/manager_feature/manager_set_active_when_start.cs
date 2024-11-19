using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager_set_active : MonoBehaviour
{
    //this script is used by manager_set_active_when_start
    //to set gameobject to active when start
    //for develop consideration

    [SerializeField] List<GameObject> object_to_be_active;

    private void Start()
    {
        for (int i = 0; i <  object_to_be_active.Count; i++)
        {
            object_to_be_active[i].SetActive(true);
        }
    }
}
