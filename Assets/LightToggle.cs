using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public GameObject Hand;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Hand.activeSelf)
            {
                Hand.SetActive(false);
            }
            else
            {
                Hand.SetActive(true);
            }
        }
    }

}