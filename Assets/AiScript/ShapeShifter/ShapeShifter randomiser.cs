using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ShapeShifterrandomiser : MonoBehaviour
{
    [SerializeField] private GameObject realBush;
    [SerializeField] private GameObject fakeBush;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        foreach(Transform point in spawnPoints)
        {
            int rand = Random.Range(1, 3);

            GameObject prefabToSpawn;
            if (rand == 1)
            {
                prefabToSpawn = realBush;
            }
            else
            {
                prefabToSpawn = fakeBush;
            }

            Instantiate(prefabToSpawn, point.position, Quaternion.identity);
        }

        
    }
}
