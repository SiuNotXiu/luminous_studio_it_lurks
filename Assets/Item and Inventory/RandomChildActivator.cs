using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChildActivator : MonoBehaviour
{
    [SerializeField] private int activeChildCount = 20; 
    private List<Transform> children = new List<Transform>();
    private bool hasRandomized = false;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
    }

    private void Start()
    {
        RandomizeActiveChildren();
    }

    public void RandomizeActiveChildren()
    {
        if (activeChildCount > children.Count)
        {
            Debug.LogWarning("activeChildCount more than number of children, changed to maximum possible/testing");
            activeChildCount = children.Count;
        }

        //starts here
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false);
        }

        //randomize
        List<int> randomIndices = new List<int>();
        while (randomIndices.Count < activeChildCount)
        {
            int randomIndex = Random.Range(0, children.Count);
            if (!randomIndices.Contains(randomIndex))
            {
                randomIndices.Add(randomIndex);
            }
        }

        // active
        foreach (int index in randomIndices)
        {
            children[index].gameObject.SetActive(true);
        }

        hasRandomized = true;
    }
}
