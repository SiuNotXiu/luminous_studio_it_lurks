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
    public bool MakeshiftCampsite;
    private int luckynum;
    public ItemData[] itemDataStroing = new ItemData[9];

    private void Start()
    {
        if(MakeshiftCampsite == false)
        {
            AssignRandomItems();
        }
    }


    public void AsssignToPlayerJournal()//active this at inventoryController
    {
        for (int i = 0; i < 9; i++) 
        {
            if (itemDataStroing[i] == null)
            {
                continue;
            }
            else
            {
                // Attempt to add the random item to the camp slot
                //Debug.Log("chestIn > " + chestIn);
                chestIn.StoreItemFromChest(itemDataStroing[i]);
            }

        }

        for (int j = 0; j < 9; j++)
        {
            itemDataStroing[j] = null;

        }

    }

    public void TakeBackFromPlayerJournal(ItemData item)
    {
        for (int i = 0; i < 9; i++)
        {
            if(itemDataStroing[i] == null)
            {
                itemDataStroing[i] =item;
                break;
            }
        }
    }
    private void AssignRandomItems()
    {

        // Determine the number of items to assign (randomly between 4 and 5)
        if(brokeCampsite)
        {
            luckynum = Random.Range(2, 4);//2~3
            ItemData randomItem = itemData[4];//bandage
            itemDataStroing[0] = randomItem;
        }
        else
        {
            luckynum = Random.Range(4, 6);//4~5
            ItemData randomItem = itemData[3];//medkit
            itemDataStroing[0] = randomItem;  
        }

        for (int i = 1; i < luckynum; i++) //-1 is to let the cmapsite got fixed specific item
        {
            // Select a random item from the itemData array
            int randomIndex = Random.Range(0, itemData.Length);
            ItemData randomItem = itemData[randomIndex];
            itemDataStroing[i] = randomItem;

        }

    }
}
