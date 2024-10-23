using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hintappear : MonoBehaviour
{
    [SerializeField] private ChestController chest;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(chest.isInRange)
        {

        }
    }
    
    private void appear()
    {

    }
}
