using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampInsideItem : MonoBehaviour
{
    // Logic for this is to assign random items (4-5) into the campsite inventory journal
    // This acts as data storage for the items

    [SerializeField] private ItemData[] itemData;
    [SerializeField] ChestInventory chestIn;
    public bool brokeCampsite;
    private int luckynum;

    private void Start()
    {
        AssignRandomItems();
    }

    private void AssignRandomItems()
    {

        // Determine the number of items to assign (randomly between 4 and 5)
        if(brokeCampsite)
        {
            luckynum = Random.Range(2, 4);//2~3
            ItemData randomItem = itemData[4];//bandage
            chestIn.StoreItemFromChest(randomItem);
        }
        else
        {
            luckynum = Random.Range(4, 6);//4~5
            ItemData randomItem = itemData[3];//medkit
            chestIn.StoreItemFromChest(randomItem);
        }

        for (int i = 0; i < luckynum-1; i++) //-1 is to let the cmapsite got fixed specific item
        {
            // Select a random item from the itemData array
            int randomIndex = Random.Range(0, itemData.Length);
            ItemData randomItem = itemData[randomIndex];

            // Attempt to add the random item to the camp slot
            chestIn.StoreItemFromChest(randomItem);
        }

    }
}
