using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager_set_active : MonoBehaviour
{
    //this script is used by manager_set_active_when_start
    //to set gameobject to active when start
    //for develop consideration
    //cj:here also use to active cornfield key random appear

    [SerializeField] List<GameObject> object_to_be_active;
    [SerializeField] List<GameObject> cornfield_key;

    private void Start()
    {
        for (int i = 0; i <  object_to_be_active.Count; i++)
        {
            object_to_be_active[i].SetActive(true);
        }

        int randomIndex = Random.Range(0, cornfield_key.Count);
        cornfield_key[randomIndex].SetActive(true);

    }
}
