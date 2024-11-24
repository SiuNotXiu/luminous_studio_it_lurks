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
    public GameObject Bundle;
    public Vector3 offset = new Vector3(1f, 0f, 0f);

    private void Start()
    {
 
        for (int i = 0; i <  object_to_be_active.Count; i++)
        {
            object_to_be_active[i].SetActive(true);
        }

        int random = Random.Range(1, 8);
        GameObject environmentObject = GameObject.Find("Environment");
        Transform cfcorpseTransform = environmentObject.transform.Find($"CornFieldCorpse/CFCorpse{random}");
        Vector3 spawnPosition = cfcorpseTransform.position + offset;

        // Spawn the key bundle
        Instantiate(Bundle, spawnPosition, Quaternion.identity);
    }
}
