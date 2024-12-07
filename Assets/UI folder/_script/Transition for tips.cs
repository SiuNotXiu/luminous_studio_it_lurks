using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Transitionfortips : MonoBehaviour
{
    public GameObject[] tips; //1: journal, 2: map
    [Header("Their father")]
    public GameObject[] father; //1: journal, 2: map

    private bool lastState = true;

    private void Update()
    {
        bool shouldShowTips = !(father[0].activeSelf || father[1].activeSelf);

        // Only update if the state has changed
        if (shouldShowTips != lastState)
        {
            foreach (GameObject child in tips)
            {
                child.SetActive(shouldShowTips);
            }
            lastState = shouldShowTips;
        }
    }
}
