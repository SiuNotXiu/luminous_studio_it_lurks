using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampInsideItem : MonoBehaviour
{
    // Logic for this is to assign random items (4-5) into the campsite inventory journal
    // This acts as data storage for the items

    [SerializeField] private ChestSlot slot;
    [SerializeField] private ItemData[] itemData;
    public bool brokeCampsite;
    private int luckynum;

    private void Start()
    {
        if (slot.isFull)
        {
            return; // Exit if the slot is full
        }

        AssignRandomItems();
    }

    private void AssignRandomItems()
    {
        // Check if the campsite slot is empty
        if (!slot.IsEmpty())
        {
            return; // Exit if the campsite slot is not empty
        }

        // Determine the number of items to assign (randomly between 4 and 5)
        if(brokeCampsite)
        {
            luckynum = Random.Range(2, 4);//2~3
        }
        else
        {
            luckynum = Random.Range(4, 6);//4~5
        }

        for (int i = 0; i < luckynum-1; i++) //-1 is to let the cmapsite got fixed specific item
        {
            // Select a random item from the itemData array
            int randomIndex = Random.Range(0, itemData.Length);
            ItemData randomItem = itemData[randomIndex];

            // Attempt to add the random item to the camp slot
            slot.AddItem(randomItem); // Add to camp slot
        }

        if(brokeCampsite)
        {
            ItemData randomItem = itemData[0];//bandage
            slot.AddItem(randomItem);
        }
        else
        {
            ItemData randomItem = itemData[0];//medkit
            slot.AddItem(randomItem);
        }
    }
}
