using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_map_ui : MonoBehaviour
{
    [SerializeField] private GameObject big_map;

    private void Start()
    {
        big_map.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (big_map != null)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (big_map.activeInHierarchy == false)
                {
                    big_map.SetActive(true);
                }
                else
                {
                    big_map.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("big_map is null");
        }
    }
}
